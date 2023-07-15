using WDA.Api.Dto.Customer.Response;
using WDA.Domain.Enums;

namespace WDA.Api.Dto.Feedback;

public class FeedbackResponse
{
    public Guid FeedbackId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public CustomerResponse Customer { get; set; } = new();
    public FeedbackStatus FeedbackStatus { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; } = DateTimeOffset.UtcNow;
    public Guid? ModifiedById { get; set; }
    public string? ModifiedByName { get; set; }
    public DateTimeOffset? CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public Guid? CreatedById { get; set; }
    public string? CreatedByName { get; set; }
}