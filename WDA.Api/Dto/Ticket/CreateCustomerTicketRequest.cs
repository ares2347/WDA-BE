using WDA.Domain.Enums;

namespace WDA.Api.Dto.Ticket;

public class CreateCustomerTicketRequest
{
    public string Content { get; set; } = string.Empty;
    public CustomerTicketCategory TicketCategory { get; set; }
    public Guid CustomerId { get; set; }
    public string ValidationToken { get; set; }
}