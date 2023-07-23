namespace WDA.Api.Dto.Ticket;

public class AssignCustomerTicketRequest
{
    public Guid TicketId { get; set; }
    public Guid? EmployeeId { get; set; }
}