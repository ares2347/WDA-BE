 using WDA.Domain.Models.User;

namespace WDA.Domain;

public abstract class BaseEntity
{
    public bool IsDelete { get; set; } = false;
    public DateTimeOffset? ModifiedAt { get; set; } = DateTimeOffset.UtcNow;
    public User? ModifiedBy { get; set; } 
    public DateTimeOffset? CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public User? CreatedBy { get; set; }
}