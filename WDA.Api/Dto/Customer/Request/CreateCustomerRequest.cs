namespace WDA.Api.Dto.Customer.Request
{
    public class CreateCustomerRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
