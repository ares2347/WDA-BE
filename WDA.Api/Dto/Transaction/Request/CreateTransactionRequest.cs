
namespace WDA.Api.Dto.Transaction.Request
{
    public class CreateTransactionRequest
    {
        public string Detail { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public Guid CustomerId { get; set; }
    }
}
