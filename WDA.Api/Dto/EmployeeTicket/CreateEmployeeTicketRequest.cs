using WDA.Domain.Enums;

namespace WDA.Api.Dto.EmployeeTicket;

public class CreateEmployeeTicketRequest
{
    public string Content { get; set; } = string.Empty;
    public EmployeeTicketCategory TicketCategory { get; set; }
    public Guid EmployeeId { get; set; }
    public string ValidationToken { get; set; } = string.Empty;
}