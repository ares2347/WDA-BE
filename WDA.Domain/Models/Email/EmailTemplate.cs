namespace WDA.Domain.Models.Email;

public class EmailTemplate
{
    public Guid EmailTemplateId { get; set; }
    public EmailTemplateType EmailTemplateType { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}

public enum EmailTemplateType
{
    TransactionCompleted,
    TicketOpened,
    TicketPending,
    TicketProcessing,
    TicketDone,
    TicketClosed,
    TicketAssigned,
    TicketReopenedRequestor,
    TicketReopenedResolver
}