namespace WDA.Domain.Models.Customer;

public class Customer : BaseEntity
{
    public Guid CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}