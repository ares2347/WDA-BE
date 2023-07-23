using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using WDA.Domain;
using WDA.Domain.Models.Email;
using WDA.Shared;

namespace WDA.Service.Email;

public class EmailService : IEmailService
{
    private readonly SmtpClient _smtp;
    private readonly AppDbContext _dbContext;

    public EmailService(SmtpClient smtp, AppDbContext dbContext)
    {
        _smtp = smtp;
        _dbContext = dbContext;
    }

    public async Task Send(string subject, string body, string toAddresses, string? ccAddresses = null,
        string? bccAddresses = null, CancellationToken cancellationToken = default)
    {
        // Set the email message details
        var message = new MailMessage();
        message.To.Add(toAddresses);

        if (!string.IsNullOrWhiteSpace(ccAddresses))
        {
            message.CC.Add(ccAddresses);
        }

        if (!string.IsNullOrWhiteSpace(bccAddresses))
        {
            message.Bcc.Add(bccAddresses);
        }

        message.Subject = subject;
        message.Body = body;
        message.IsBodyHtml = true;
        message.From = new MailAddress(AppSettings.Instance.Smtp.From, AppSettings.Instance.Smtp.DisplayName);

        // Send the email
        await _smtp.SendMailAsync(message, cancellationToken);
    }
    
    public async Task<EmailTemplate?> GetEmailTemplate(EmailTemplateType emailTemplateType,
        CancellationToken cancellationToken)
    {
        return await _dbContext.EmailTemplates
            .FirstOrDefaultAsync(x => x.EmailTemplateType == emailTemplateType, cancellationToken)
            .ConfigureAwait(false);
    }
}