namespace WDA.Domain;

public abstract class BaseEntity
{
    public bool IsDelete { get; set; } = false;
    public DateTimeOffset? ModifiedAt { get; set; }
    public User.User? ModifiedBy { get; set; }    
    public DateTimeOffset? CreatedAt { get; set; }
    public User.User? CreatedBy { get; set; }
}