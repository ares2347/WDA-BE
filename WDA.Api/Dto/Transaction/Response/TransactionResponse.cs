using System.Transactions;
using WDA.Api.Dto.Customer.Response;
using WDA.Api.Dto.Transaction.Request;

namespace WDA.Api.Dto.Transaction.Response
{
    public class TransactionResponse
    {
        public Guid TransactionId { get; set; }
        public string Detail { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public TransactionStatus Status { get; set; }
        public CustomerResponse Customer { get; set; } = new();
        public Guid? ModifiedById { get; set; }
        public string? ModifiedByName { get; set; }
        public string? ModifiedAt { get; set; }
        public DateTimeOffset? CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public Guid? CreatedById { get; set; }
        public string? CreatedByName { get; set; }
    }
}
