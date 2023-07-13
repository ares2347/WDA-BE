using WDA.Api.Dto.Customer.Response;
using WDA.Api.Dto.Transaction.Request;
using WDA.Domain.Models.Remark;
using WDA.Domain.Models.Transaction;

namespace WDA.Api.Dto.Transaction.Response
{
    public class TransactionResponse
    {
        public Guid TransactionId { get; set; }
        public decimal Total { get; set; }
        public string Date { get; set; } = string.Empty;
        public List<SubTransactionDto> SubTransactions { get; set; } = new();
        public List<RemarksDto> Remarks { get; set; } = new();
        public CustomerResponse Customer { get; set; } = new();
        public Guid? ModifiedById { get; set; }
        public string? ModifiedByName { get; set; }
        public DateTimeOffset? CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public Guid? CreatedById { get; set; }
        public string? CreatedByName { get; set; }
    }
}