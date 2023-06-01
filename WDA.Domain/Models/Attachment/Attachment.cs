namespace WDA.Domain.Models.Attachment;

public class Attachment : BaseEntity
{
    public Guid AttachmentId { get; set; }
    public string Path { get; set; } = string.Empty;
    public long Size { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
}