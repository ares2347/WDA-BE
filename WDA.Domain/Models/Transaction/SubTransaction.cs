namespace WDA.Domain.Models.Transaction;

public class SubTransaction : BaseEntity
{
    public Guid SubTransactionId { get; set; }
    public string Details { get; set; } = string.Empty;
    public decimal SubTotal { get; set; } = 0;

    public Transaction Transaction { get; set; } = new();
}