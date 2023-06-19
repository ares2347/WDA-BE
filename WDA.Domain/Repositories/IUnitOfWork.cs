using WDA.Domain.Models.Customer;
using WDA.Domain.Models.Document;
using WDA.Domain.Models.Transaction;

namespace WDA.Domain.Repositories;

public interface IUnitOfWork
{
    IBaseRepository<Customer> CustomerRepository { get; }
    IBaseRepository<Transaction> TransactionRepository { get; }
    IBaseRepository<Document> DocumentRepository { get; }

    Task<bool> SaveChangesAsync();
}
