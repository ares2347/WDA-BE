using WDA.Domain.Models.Attachment;
using WDA.Domain.Models.Customer;
using WDA.Domain.Models.Document;
using WDA.Domain.Models.Feedback;
using WDA.Domain.Models.Thread;
using WDA.Domain.Models.Transaction;
using Thread = WDA.Domain.Models.Thread.Thread;

namespace WDA.Domain.Repositories;

public interface IUnitOfWork
{
    IBaseRepository<Customer> CustomerRepository { get; }
    IBaseRepository<Transaction> TransactionRepository { get; }
    IBaseRepository<Document> DocumentRepository { get; }
    IBaseRepository<Attachment> AttachmentRepository { get; }
    IBaseRepository<Thread> ThreadRepository { get; }
    IBaseRepository<Reply> ReplyRepository { get; }
    IBaseRepository<Feedback> FeedbackRepository { get; }
    AdminRepository AdminRepository { get; }

    Task<bool> SaveChangesAsync(CancellationToken _ = default);
}
