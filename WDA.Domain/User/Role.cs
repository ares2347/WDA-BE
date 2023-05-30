using Microsoft.AspNetCore.Identity;

namespace WDA.Domain.User;

public class Role : IdentityRole<Guid>
{
    public User? CreatedBy { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public User? ModifiedBy { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
}