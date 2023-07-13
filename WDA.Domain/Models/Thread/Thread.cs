namespace WDA.Domain.Models.Thread;

public class Thread : BaseEntity
{
    public Guid ThreadId { get; set; }
    public string Topic { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<Reply> Replies { get; set; } = new();
}