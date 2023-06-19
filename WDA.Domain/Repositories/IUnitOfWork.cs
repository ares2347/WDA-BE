using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDA.Domain.Models.Customer;
using WDA.Domain.Models.Document;
using WDA.Domain.Models.Transaction;
using WDA.Domain.Models.User;

namespace WDA.Domain.Repositories;

public interface IUnitOfWork
{
    IBaseRepository<Customer> CustomerRepository { get; }
    IBaseRepository<Transaction> TransactionRepository { get; }
    IBaseRepository<Document> DocumentRepository { get; }

    Task<bool> SaveChanges();
}
