namespace WDA.Api.Dto.CustomerTicket;

public class CloseCustomerTicketRequest
{
    public Guid TicketId { get; set; }
    public string? ValidationToken { get; set; }
}