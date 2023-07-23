using System.Security.Claims;

namespace WDA.Shared;

public class UserContext
{
    public Guid UserId { get; private set; }
    public string? Email { get; private set; }
    public IEnumerable<string> Roles { get; private set; }

    public static UserContext Build(ClaimsPrincipal? claimsPrincipal)
    {
        HttpException.ThrowIfNull(claimsPrincipal);
        var emailClaim =
            claimsPrincipal!.Claims.FirstOrDefault(x =>
                x.Type.Equals(ClaimTypes.Email, StringComparison.OrdinalIgnoreCase));
        var userIdClaim = claimsPrincipal.Claims.FirstOrDefault(x =>
            x.Type.Equals(ClaimTypes.Sid, StringComparison.OrdinalIgnoreCase));        
        var roleClaims = claimsPrincipal.Claims.Where(x =>
            x.Type.Equals(ClaimTypes.Role, StringComparison.OrdinalIgnoreCase));

        var roles = roleClaims.Select(x => x.Value);
        var email = emailClaim?.Value;
        _ = Guid.TryParse(userIdClaim?.Value, out var userId);        
        
        return new UserContext
        {
            Email = email,
            UserId = userId,
            Roles = roles
        };
    }    
}
