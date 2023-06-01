namespace WDA.Domain.Models.Document;

public class Document : BaseEntity
{
    public Guid DocumentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string SubCategory { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}