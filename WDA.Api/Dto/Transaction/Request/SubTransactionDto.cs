namespace WDA.Api.Dto.Transaction.Request
{
    public class SubTransactionDto
    {
        public string Details { get; set; } = string.Empty;
        public decimal SubTotal { get; set; } = 0;
    }
}
