using WDA.Domain.Models.Email;

namespace WDA.Service.Email;

public interface IEmailService
{
    Task Send(string subject, string body, string toAddresses, string? ccAddresses = null, string? bccAddresses = null,
        CancellationToken cancellationToken = default);
    Task<EmailTemplate?> GetEmailTemplate(EmailTemplateType emailTemplateType, CancellationToken cancellationToken);
}