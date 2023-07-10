namespace WDA.Api.Dto.Attachment;

public class AttachmentResponse
{
    public Guid AttachmentId { get; set; }
    public string Path { get; set; } = string.Empty;
    public long Size { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public DateTimeOffset? ModifiedAt { get; set; } = DateTimeOffset.UtcNow;
    public Guid? ModifiedById { get; set; }
    public string? ModifiedByName { get; set; }
    public DateTimeOffset? CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public Guid? CreatedById { get; set; }
    public string? CreatedByName { get; set; }
}