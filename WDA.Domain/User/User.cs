using Microsoft.AspNetCore.Identity;

namespace WDA.Domain.User;

public class User : IdentityUser<Guid>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FullName { get; set; }
    public bool PasswordChangeRequired { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
}