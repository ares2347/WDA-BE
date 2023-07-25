namespace WDA.Api.Dto.CustomerTicket;

public class ReopenCustomerTicketRequest
{
    public Guid TicketId { get; set; }
    public string Content { get; set; }
    public string? ValidationToken { get; set; }
}