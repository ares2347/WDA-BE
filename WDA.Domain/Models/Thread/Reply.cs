namespace WDA.Domain.Models.Thread;

public class Reply : BaseEntity
{
    public Guid ReplyId { get; set; }
    public Thread Thread { get; set; } = new();
    public string Content { get; set; } = string.Empty;
    public List<Reply> Replies { get; set; } = new();
}