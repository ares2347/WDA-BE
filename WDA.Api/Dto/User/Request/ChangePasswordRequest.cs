namespace WDA.Api.Dto.User.Request;

public class ChangePasswordRequest
{
    public Guid UserId { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}