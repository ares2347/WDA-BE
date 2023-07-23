namespace WDA.Api.Dto.User.Request;

public class CreateNewAccountRequest
{
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string Department { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
}