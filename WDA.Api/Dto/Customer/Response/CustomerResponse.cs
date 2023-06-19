namespace WDA.Api.Dto.Customer.Response
{
    public class CustomerResponse
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTimeOffset? ModifiedAt { get; set; } = DateTimeOffset.UtcNow;
        public Guid? ModifiedById { get; set; }
        public string? ModifiedByName { get; set; }
        public DateTimeOffset? CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public Guid? CreatedById { get; set; }
        public string? CreatedByName { get; set; }
    }
}
