using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using WDA.Domain.Models.Ticket;

namespace WDA.Domain.Models.User;

public class User : IdentityUser<Guid>
{
    public string? FullName { get; set; }
    public Guid? ProfilePictureId { get; set; }
    public string Department { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    
    public bool PasswordChangeRequired { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    [NotMapped] public List<string> Roles { get; set; } = new();
}