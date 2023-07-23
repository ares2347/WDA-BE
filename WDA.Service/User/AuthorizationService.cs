using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WDA.Domain.Models.User;
using WDA.Shared;

namespace WDA.Service.User;

public class AuthorizationService : IAuthorizationService
{
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly UserManager<Domain.Models.User.User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public AuthorizationService(JwtSecurityTokenHandler tokenHandler, UserManager<Domain.Models.User.User> userManager, RoleManager<Domain.Models.User.Role> roleManager)
    {
        _tokenHandler = tokenHandler;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<TokenResult?> IssueToken(Domain.Models.User.User user, CancellationToken _ = default)
    {
        var claims = new List<Claim>();

        var roles = await _userManager.GetRolesAsync(user) ?? Enumerable.Empty<string>();

        //TODO investigate JWT claims
        claims.Add(new Claim(ClaimTypes.Email, user.Email ?? string.Empty));
        claims.Add(new Claim(ClaimTypes.Sid, user.Id.ToString()));
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var signingKeys = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.Instance.Jwt.Secret));

        var token = new JwtSecurityToken(
            AppSettings.Instance.Jwt.ValidIssuer,
            AppSettings.Instance.Jwt.ValidAudience,
            expires: DateTime.Now.AddDays(7),
            claims: claims,
            signingCredentials: new SigningCredentials(signingKeys, SecurityAlgorithms.HmacSha256));

        var tokenString= _tokenHandler.WriteToken(token);
        return new TokenResult(tokenString,token.ValidTo );
    }

    public TokenResult IssueTemporaryToken(int duration, CancellationToken _ = default)
    {
        var claims = new List<Claim>();
        
        var signingKeys = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.Instance.Jwt.Secret));

        var token = new JwtSecurityToken(
            AppSettings.Instance.Jwt.ValidIssuer,
            AppSettings.Instance.Jwt.ValidAudience,
            expires: DateTime.Now.AddDays(duration),
            claims: claims,
            signingCredentials: new SigningCredentials(signingKeys, SecurityAlgorithms.HmacSha256));

        var tokenString= _tokenHandler.WriteToken(token);
        return new TokenResult(tokenString,token.ValidTo );
    }

    public TokenResult ValidateToken(string token)
    {
        try
        {
            _tokenHandler.ValidateToken(token, GetJwtTokenValidationParameters(true), out _);
            var decryptedToken = _tokenHandler.ReadJwtToken(token);
            return new TokenResult(token, decryptedToken.ValidTo);
        }
        catch (Exception e)
        {
            throw new HttpException("The current user is not authorized to access this resource.", HttpStatusCode.Unauthorized);
        }
    }

    public async Task<TokenResult?> AuthorizeUser(string identifier, string password, CancellationToken _ = default)
    {
        var user = await _userManager.FindByNameAsync(identifier) ?? await _userManager.FindByEmailAsync(identifier);
        if (user != null && await _userManager.CheckPasswordAsync(user, password))
        {
            return await IssueToken(user, _);
        }

        throw new HttpException("Invalid email or password", HttpStatusCode.Unauthorized);
    }

    public async Task<Domain.Models.User.User?> RegisterUser(Domain.Models.User.User user, List<string> roles, string? password, CancellationToken _ = default)
    {
        foreach (var role in roles)
        {
            var existedRole = await _roleManager.FindByNameAsync(role);
            if (existedRole is null) throw new HttpException("Invalid role name", HttpStatusCode.BadRequest);
        }
        
        
        //TODO refactor default password behavior
        var result = password is null ? await _userManager.CreateAsync(user) : await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            throw new HttpException("User registration failed!", HttpStatusCode.BadRequest);
        }
        await _userManager.AddToRolesAsync(user, roles);
        var authUser = await _userManager.FindByNameAsync(user.UserName ?? string.Empty) ?? await _userManager.FindByEmailAsync(user.Email ?? string.Empty);
        return authUser;
    }
    
    public async Task<IdentityResult> ChangePassword(Guid userId, string oldPassword, string newPassword, CancellationToken _ = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null) throw new HttpException("User not found.", HttpStatusCode.NotFound);
        var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        return result;
    }
    public static TokenValidationParameters GetJwtTokenValidationParameters(bool validateLifeTime = true)
    {
        return new TokenValidationParameters
        {
            ValidIssuer = AppSettings.Instance.Jwt.ValidIssuer,
            ValidAudience = AppSettings.Instance.Jwt.ValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.Instance.Jwt.Secret)),
            ValidateIssuerSigningKey = true,
            ValidateLifetime = validateLifeTime,
            ValidateIssuer = true,
            ValidateAudience = true
        };
    }
}

