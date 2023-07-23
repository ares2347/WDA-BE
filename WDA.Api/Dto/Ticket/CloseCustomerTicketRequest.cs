namespace WDA.Api.Dto.Ticket;

public class CloseCustomerTicketRequest
{
    public Guid TicketId { get; set; }
    public string? ValidationToken { get; set; }
}