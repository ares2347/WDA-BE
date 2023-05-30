using Microsoft.AspNetCore.Identity;

namespace WDA.Service.User;

public interface IAuthorizationService
{
    Task<string?> AuthorizeUser(string identifier, string password, CancellationToken _ = default);
    Task<string?> RegisterUser(string? username, string email, string password, string firstName, string lastName, List<string> roles, CancellationToken _ = default);
    Task<IdentityResult> ChangePassword(Guid userId, string oldPassword, string newPassword, CancellationToken _ = default);
}