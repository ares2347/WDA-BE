using System.Net;
using System.Net.Mail;
using System.Text;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using WDA.Domain;
using WDA.Domain.Models.Email;
using WDA.Shared;

namespace WDA.Service.Email;

public class EmailService : IEmailService
{
    private readonly SmtpClient _smtp;
    private readonly AppDbContext _dbContext;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public EmailService(SmtpClient smtp, AppDbContext dbContext, IBackgroundJobClient backgroundJobClient)
    {
        _smtp = smtp;
        _dbContext = dbContext;
        _backgroundJobClient = backgroundJobClient;
    }

    public async Task SendEmailNotification(EmailTemplateType templateType,string receiverEmail, Dictionary<string, string>? subjectReplacements, Dictionary<string, string>? bodyReplacements,
         CancellationToken _ = default)
    {
        var template = await GetEmailTemplate(templateType, _);
        if (template is null) throw new HttpException("Email Template Not Found.", HttpStatusCode.InternalServerError);
        var subjectBuilder = new StringBuilder(template.Subject);
        if (subjectReplacements != null)
            foreach (var pair in subjectReplacements)
            {
                subjectBuilder.Replace($"[[{pair.Key}]]", pair.Value);
            }
        var bodyBuilder = new StringBuilder(template.Body);
        if (bodyReplacements != null)
            foreach (var pair in bodyReplacements)
            {
                bodyBuilder.Replace($"[[{pair.Key}]]", pair.Value);
            }
        _backgroundJobClient.Enqueue(() =>
            Send(subjectBuilder.ToString(), bodyBuilder.ToString(),receiverEmail,null,null,_));
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