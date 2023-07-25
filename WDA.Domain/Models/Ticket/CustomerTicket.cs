using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WDA.Domain.Enums;

namespace WDA.Domain.Models.Ticket;

public class CustomerTicket
{
    [Key]
    public Guid TicketId { get; set; }
    public string Content { get; set; } = string.Empty;
    public TicketStatus Status { get; set; } = TicketStatus.Opened;
    public bool IsDeleted { get; set; } = false;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset LastModified { get; set; } = DateTimeOffset.UtcNow;
    public CustomerTicketCategory TicketCategory { get; set; }
    public List<string> ReopenReasons { get; set; } = new();
    [ForeignKey("Requestor")] public Guid RequestorId { get; set; }
    public Customer.Customer Requestor { get; set; } = new();
    
    [ForeignKey("Resolver")] public Guid ResolverId { get; set; }
    public User.User? Resolver { get; set; } = new();

}