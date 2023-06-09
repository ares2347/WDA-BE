namespace WDA.Api.Dto.User.Request;

public class RegisterRequest
{
    public string? Username { get; set; }
    public string Email { get; set; }
    public string? Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Department { get; set; }
    // public string Position { get; set; }
    public List<string> Roles { get; set; }
}