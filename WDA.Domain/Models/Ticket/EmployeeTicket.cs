using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WDA.Domain.Enums;

namespace WDA.Domain.Models.Ticket;

public class EmployeeTicket
{
    [Key]
    public Guid TicketId { get; set; }
    public string Content { get; set; } = string.Empty;
    public TicketStatus Status { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public EmployeeTicketCategory TicketCategory { get; set; }
    public List<string> ReopenReasons { get; set; } = new();
    [ForeignKey("Resolver")] public Guid ResolverId { get; set; }
    public User.User? Resolver { get; set; } = new();
    
    [ForeignKey("Requestor")] public Guid RequestorId { get; set; }
    public User.User Requestor { get; set; } = new();
}