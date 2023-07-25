using WDA.Api.Dto.User.Response;
using WDA.Domain.Enums;

namespace WDA.Api.Dto.EmployeeTicket;

public class EmployeeTicketResponse
{
    public Guid TicketId { get; set; }
    public string Content { get; set; } = string.Empty;
    public TicketStatus Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public EmployeeTicketCategory TicketCategory { get; set; }
    public UserInfoResponse Requestor { get; set; } = new();
    public UserInfoResponse? Resolver { get; set; }
}