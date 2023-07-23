using WDA.Api.Dto.Attachment;

namespace WDA.Api.Dto.User.Response
{
    public class UserInfoResponse
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FullName { get; set; }
        public string? EmployeeCode { get; set; }
        public string Department { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public bool PasswordChangeRequired { get; set; } = true;
        public Guid? ProfilePictureId { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
