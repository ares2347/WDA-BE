using WDA.Domain.Models.User;

namespace WDA.Domain;

public abstract class BaseEntity
{
    public bool IsDelete { get; set; } = false;
    public DateTimeOffset? ModifiedAt { get; set; }
    public User? ModifiedBy { get; set; }    
    public DateTimeOffset? CreatedAt { get; set; }
    public User? CreatedBy { get; set; }
}