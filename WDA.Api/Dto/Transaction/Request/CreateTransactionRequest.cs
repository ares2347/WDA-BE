
namespace WDA.Api.Dto.Transaction.Request
{
    public class CreateTransactionRequest
    {
        public List<SubTransactionDto> SubTransactions { get; set; } = new();
        public List<RemarksDto> Remarks { get; set; } = new();
        public Guid CustomerId { get; set; } = new();
    }
}
