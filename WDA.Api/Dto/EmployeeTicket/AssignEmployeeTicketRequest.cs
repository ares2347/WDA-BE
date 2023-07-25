namespace WDA.Api.Dto.EmployeeTicket;

public class AssignEmployeeTicketRequest
{
    public Guid TicketId { get; set; }
    public Guid? EmployeeId { get; set; }
}