namespace WDA.Api.Dto.CustomerTicket;

public class AssignCustomerTicketRequest
{
    public Guid TicketId { get; set; }
    public Guid? EmployeeId { get; set; }
}