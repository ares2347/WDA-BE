using Microsoft.AspNetCore.Identity;
using WDA.Domain.Enums;
using WDA.Domain.Models.Email;
using WDA.Domain.Models.User;

namespace WDA.Domain;

public static class BuiltInData
{
    private const string UserFullName = "System Admin";
    private const string UserEmail = "admin@email.com";
    private const string UserIdentifier = "admin";


    public static List<User> SeedUserData() => new List<User>
    {
        new User
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
            UserName = UserIdentifier,
            FullName = UserFullName,
            NormalizedUserName = UserIdentifier.Normalize(),
            Email = UserEmail,
            EmailConfirmed = true,
            NormalizedEmail = UserEmail.Normalize(),
            PasswordHash = "AQAAAAIAAYagAAAAEMeRmOWs9W/KsBTc0NEYwk5Efsp1rjs48fPIPWSW0xhuuKWByjTRlnJXKrEmn9yPhA==",
            SecurityStamp = "3YYPM246ONSVZFAKY3TR2JSVKMX7ZM4D",
            ConcurrencyStamp = "6a9943f8-af5b-4231-9a8d-63f8c43c6e0c",
            PasswordChangeRequired = false
        }
    };

    public static List<Role> SeedRoleData() => new List<Role>
    {
        new Role
        {
            Name = RoleName.Admin,
            NormalizedName = RoleName.Admin.Normalize(),
            Id = Guid.Parse("20000000-0000-0000-0000-000000000001")
        },
        new Role
        {
            Name = RoleName.Hr,
            NormalizedName = RoleName.Hr.Normalize(),
            Id = Guid.Parse("20000000-0000-0000-0000-000000000002")
        },
        new Role
        {
            Name = RoleName.Sale,
            NormalizedName = RoleName.Sale.Normalize(),
            Id = Guid.Parse("20000000-0000-0000-0000-000000000003")
        },
        new Role
        {
            Name = RoleName.HrManager,
            NormalizedName = RoleName.HrManager.Normalize(),
            Id = Guid.Parse("20000000-0000-0000-0000-000000000004")
        },
        new Role
        {
            Name = RoleName.SaleManager,
            NormalizedName = RoleName.SaleManager.Normalize(),
            Id = Guid.Parse("20000000-0000-0000-0000-000000000005")
        },
    };

    public static List<IdentityUserRole<Guid>> SeedUserRoles() => new List<IdentityUserRole<Guid>>
    {
        new IdentityUserRole<Guid>
        {
            RoleId = Guid.Parse("20000000-0000-0000-0000-000000000001"),
            UserId = Guid.Parse("10000000-0000-0000-0000-000000000001")
        }
    };

    public static List<EmailTemplate> SeedEmailTemplates() => new()
    {
        new EmailTemplate
        {
            EmailTemplateId = Guid.Parse("30000000-0000-0000-0000-000000000001"),
            EmailTemplateType = EmailTemplateType.TransactionCompleted,
            Subject = "Transaction No. [[TransactionId]] has been completed.",
            Body = GetText($"{AppContext.BaseDirectory}/Models/Email/BuiltInTemplates/TransactionCompleted.txt")
        },
        new EmailTemplate
        {
            EmailTemplateId = Guid.Parse("30000000-0000-0000-0000-000000000002"),
            EmailTemplateType = EmailTemplateType.TicketOpened,
            Subject = "Ticket No. [[TicketId]] has been opened.",
            Body = GetText($"{AppContext.BaseDirectory}/Models/Email/BuiltInTemplates/TicketOpened.txt")
        },
        new EmailTemplate
        {
            EmailTemplateId = Guid.Parse("30000000-0000-0000-0000-000000000003"),
            EmailTemplateType = EmailTemplateType.TicketPending,
            Subject = "Ticket No. [[TicketId]] has been assigned.",
            Body = GetText($"{AppContext.BaseDirectory}/Models/Email/BuiltInTemplates/TicketPending.txt")
        },
        new EmailTemplate
        {
            EmailTemplateId = Guid.Parse("30000000-0000-0000-0000-000000000004"),
            EmailTemplateType = EmailTemplateType.TicketProcessing,
            Subject = "Ticket No. [[TicketId]] is being processed.",
            Body = GetText($"{AppContext.BaseDirectory}/Models/Email/BuiltInTemplates/TicketProcessing.txt")
        },
        new EmailTemplate
        {
            EmailTemplateId = Guid.Parse("30000000-0000-0000-0000-000000000005"),
            EmailTemplateType = EmailTemplateType.TicketDone,
            Subject = "Ticket No. [[TicketId]] has been completed.",
            Body = GetText($"{AppContext.BaseDirectory}/Models/Email/BuiltInTemplates/TicketDone.txt")
        },
        new EmailTemplate
        {
            EmailTemplateId = Guid.Parse("30000000-0000-0000-0000-000000000006"),
            EmailTemplateType = EmailTemplateType.TicketClosed,
            Subject = "Ticket No. [[TicketId]] has been closed.",
            Body = GetText($"{AppContext.BaseDirectory}/Models/Email/BuiltInTemplates/TicketClosed.txt")
        },
        new EmailTemplate
        {
            EmailTemplateId = Guid.Parse("30000000-0000-0000-0000-000000000007"),
            EmailTemplateType = EmailTemplateType.TicketAssigned,
            Subject = "Ticket No. [[TicketId]] has been assigned to you.",
            Body = GetText($"{AppContext.BaseDirectory}/Models/Email/BuiltInTemplates/NotImplemented.txt")
        },
        new EmailTemplate
        {
            EmailTemplateId = Guid.Parse("30000000-0000-0000-0000-000000000008"),
            EmailTemplateType = EmailTemplateType.TicketReopenedRequestor,
            Subject = "Ticket No. [[TicketId]] has been reopened.",
            Body = GetText($"{AppContext.BaseDirectory}/Models/Email/BuiltInTemplates/NotImplemented.txt")
        },
        new EmailTemplate
        {
            EmailTemplateId = Guid.Parse("30000000-0000-0000-0000-000000000009"),
            EmailTemplateType = EmailTemplateType.TicketReopenedResolver,
            Subject = "Ticket No. [[TicketId]] has been reopened by Requestor.",
            Body = GetText($"{AppContext.BaseDirectory}/Models/Email/BuiltInTemplates/NotImplemented.txt")
        }
    };

    #region private method

    private static string GetText(string filePath)
    {
        return File.ReadAllText(filePath);
    }

    #endregion
}