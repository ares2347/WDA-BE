namespace WDA.Api.Dto.Forum;

public class ThreadResponse
{
    public Guid ThreadId { get; set; }
    public string Topic { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<ReplyResponse> Replies { get; set; } = new();
    public DateTimeOffset? ModifiedAt { get; set; } = DateTimeOffset.UtcNow;
    public Guid? ModifiedById { get; set; }
    public string? ModifiedByName { get; set; }
    public DateTimeOffset? CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public Guid? CreatedById { get; set; }
    public string? CreatedByName { get; set; }
}