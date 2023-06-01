using WDA.Domain.Enums;

namespace WDA.Domain.Models.Transaction;

public class Transaction : BaseEntity
{
    public Guid TransactionId { get; set; }
    public decimal Total { get; set; }
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTimeOffset.UtcNow.DateTime);
    public List<SubTransaction> SubTransactions { get; set; } = new();
    public List<Remark.Remark> Remarks { get; set; } = new();
    public TransactionStatus Status { get; set; }

    public Customer.Customer Customer { get; set; } = new();
}