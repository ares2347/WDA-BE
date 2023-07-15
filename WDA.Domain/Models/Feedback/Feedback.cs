using WDA.Domain.Enums;

namespace WDA.Domain.Models.Feedback;

public class Feedback : BaseEntity
{
    public Guid FeedbackId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Customer.Customer Customer { get; set; } = new();
    public FeedbackStatus FeedbackStatus { get; set; }
}