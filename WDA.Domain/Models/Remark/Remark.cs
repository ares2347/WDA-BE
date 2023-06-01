namespace WDA.Domain.Models.Remark;

public class Remark : BaseEntity
{
    public Guid RemarkId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    
    public Customer.Customer Customer { get; set; } = new();
}