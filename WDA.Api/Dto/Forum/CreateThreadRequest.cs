namespace WDA.Api.Dto.Forum;

public class CreateThreadRequest
{
    public string Topic { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}