namespace WDA.Api.Dto.Feedback;

public class CreateFeedbackRequest
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
}