using WDA.Domain.Models.Email;

namespace WDA.Service.Email;

public interface IEmailService
{
    Task SendEmailNotification(EmailTemplateType templateType,string receiverEmail, Dictionary<string, string>? subjectReplacements, Dictionary<string, string>? bodyReplacements,
        CancellationToken _ = default);
}