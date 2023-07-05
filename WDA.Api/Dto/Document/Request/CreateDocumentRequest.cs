namespace WDA.Api.Dto.Document.Request;

public class CreateDocumentRequest
{
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string SubCategory { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}