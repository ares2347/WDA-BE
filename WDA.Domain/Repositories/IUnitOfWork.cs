using WDA.Domain.Models.Attachment;
using WDA.Domain.Models.Customer;
using WDA.Domain.Models.Transaction;

namespace WDA.Domain.Repositories;

public interface IUnitOfWork
{
    IBaseRepository<Customer> CustomerRepository { get; }
    IBaseRepository<Transaction> TransactionRepository { get; }
    IBaseRepository<Attachment> AttachmentRepository { get; }
    AdminRepository AdminRepository { get; }
    TicketRepository TicketRepository { get; }

    Task<bool> SaveChangesAsync(CancellationToken _ = default);
}
