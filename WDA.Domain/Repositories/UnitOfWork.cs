using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDA.Domain.Models.Customer;
using WDA.Domain.Models.Document;
using WDA.Domain.Models.Transaction;

namespace WDA.Domain.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private readonly AppDbContext _dbContext;
    private IBaseRepository<Customer> _customerRepository;
    private IBaseRepository<Transaction> _transactionRepository;
    private IBaseRepository<Document> _documentRepository;

    public IBaseRepository<Customer> CustomerRepository
    {
        get
        {
            if (_customerRepository is null)
            {
                _customerRepository = new CustomerRepository(_dbContext);
            }
            return _customerRepository;
        }
    }

    public IBaseRepository<Transaction> TransactionRepository
    {
        get
        {
            if (_transactionRepository is null)
            {
                _transactionRepository = new TransactionRepository(_dbContext);
            }
            return _transactionRepository;
        }
    }

    public IBaseRepository<Document> DocumentRepository
    {
        get
        {
            if (_documentRepository is null)
            {
                _documentRepository = new DocumentRepository(_dbContext);
            }
            return _documentRepository;
        }
    }


    public async Task<bool> SaveChanges()
    {
        return await _dbContext.SaveChangesAsync() > 0;
    }
}
