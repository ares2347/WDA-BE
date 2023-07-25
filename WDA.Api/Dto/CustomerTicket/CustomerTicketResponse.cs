using WDA.Api.Dto.Customer.Response;
using WDA.Api.Dto.User.Response;
using WDA.Domain.Enums;

namespace WDA.Api.Dto.CustomerTicket;

public class CustomerTicketResponse
{
    public Guid TicketId { get; set; }
    public string Content { get; set; } = string.Empty;
    public TicketStatus Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public CustomerTicketCategory TicketCategory { get; set; }
    public CustomerResponse Requestor { get; set; }
    public UserInfoResponse? Resolver { get; set; }
}