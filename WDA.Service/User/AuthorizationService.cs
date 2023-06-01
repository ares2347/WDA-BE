using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WDA.Shared;

namespace WDA.Service.User;

public class AuthorizationService : IAuthorizationService
{
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly UserManager<Domain.Models.User.User> _userManager;

    public AuthorizationService(JwtSecurityTokenHandler tokenHandler, UserManager<Domain.Models.User.User> userManager)
    {
        _tokenHandler = tokenHandler;
        _userManager = userManager;
    }

    private async Task<string?> IssueToken(Domain.Models.User.User user, CancellationToken _ = default)
    {
        var claims = new List<Claim>();

        var roles = await _userManager.GetRolesAsync(user) ?? Enumerable.Empty<string>();

        //TODO investigate JWT claims
        claims.Add(new Claim(ClaimTypes.Email, user.Email ?? string.Empty));
        claims.Add(new Claim(ClaimTypes.Sid, user.Id.ToString()));
        claims.Add(new Claim(ClaimTypes.Name, user.UserName ?? string.Empty));
        claims.Add(new Claim("FirstName", user.FirstName ?? string.Empty));
        claims.Add(new Claim("LastName", user.LastName ?? string.Empty));
        claims.Add(new Claim("FullName", user.FullName ?? string.Empty));
        
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var signingKeys = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.Instance.Jwt.Secret));

        var token = new JwtSecurityToken(
            AppSettings.Instance.Jwt.ValidIssuer,
            AppSettings.Instance.Jwt.ValidAudience,
            expires: DateTime.Now.AddDays(7),
            claims: claims,
            signingCredentials: new SigningCredentials(signingKeys, SecurityAlgorithms.HmacSha256));

        return _tokenHandler.WriteToken(token);
    }

    public async Task<string?> AuthorizeUser(string identifier, string password, CancellationToken _ = default)
    {
        var user = await _userManager.FindByNameAsync(identifier) ?? await _userManager.FindByEmailAsync(identifier);
        if (user != null && await _userManager.CheckPasswordAsync(user, password))
        {
            return await IssueToken(user, _);
        }

        throw new HttpException("Invalid email or password", HttpStatusCode.Unauthorized);
    }

    public async Task<string?> RegisterUser(string? username, string email, string? password, string firstName, string lastName, List<string> roles, CancellationToken _ = default)
    {
        var user = new Domain.Models.User.User()
        {
            UserName = username ?? email,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            // Only force users to change password at first login if the account password is not identified
            PasswordChangeRequired = password is null,
        };
        
        //TODO refactor default password behavior
        var result = await _userManager.CreateAsync(user, password ?? "password");

        if (!result.Succeeded)
        {
            throw new HttpException("User registration failed!", HttpStatusCode.BadRequest);
        }
        await _userManager.AddToRolesAsync(user, roles);
        var authUser = await _userManager.FindByNameAsync(username ?? string.Empty) ?? await _userManager.FindByEmailAsync(email);
        return await IssueToken(user, _);
    }
    
    public async Task<IdentityResult> ChangePassword(Guid userId, string oldPassword, string newPassword, CancellationToken _ = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null) throw new HttpException("User not found.", HttpStatusCode.NotFound);
        var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        return result;
    }
}