using System.ComponentModel.DataAnnotations.Schema;
using WDA.Domain.Enums;
using WDA.Domain.Models.Ticket;

namespace WDA.Domain.Models.Transaction;

public class Transaction : BaseEntity
{
    public Guid TransactionId { get; set; }
    public string Detail { get; set; } = string.Empty;
    public decimal Total { get; set; }

    [ForeignKey("Customer")]
    public Guid CustomerId { get; set; }
    public Customer.Customer Customer { get; set; } = new();
}