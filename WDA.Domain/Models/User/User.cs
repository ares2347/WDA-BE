using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WDA.Domain.Models.User;

public class User : IdentityUser<Guid>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FullName { get; set; }
    public string? EmployeeCode { get; set; }
    public Guid? ProfilePictureId { get; set; }
    public string Department { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Counter { get; set; }
    public bool PasswordChangeRequired { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    [NotMapped] public List<string> Roles { get; set; } = new();
}