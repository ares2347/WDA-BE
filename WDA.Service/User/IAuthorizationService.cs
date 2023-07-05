using Microsoft.AspNetCore.Identity;

namespace WDA.Service.User;

public interface IAuthorizationService
{
    Task<string?> AuthorizeUser(string identifier, string password, CancellationToken _ = default);
    Task<Domain.Models.User.User?> RegisterUser(Domain.Models.User.User user, List<string> roles, string? password, CancellationToken _ = default);
    Task<IdentityResult> ChangePassword(Guid userId, string oldPassword, string newPassword, CancellationToken _ = default);
    Task<string?> IssueToken(Domain.Models.User.User user, CancellationToken _ = default);
}