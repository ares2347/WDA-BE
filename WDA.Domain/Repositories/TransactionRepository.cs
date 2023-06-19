using System.Linq.Expressions;
using WDA.Domain.Models.Transaction;
using WDA.Domain.Models.User;

namespace WDA.Domain.Repositories;

public class TransactionRepository : IBaseRepository<Transaction>
{
    private readonly AppDbContext _dbContext;

    public TransactionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Transaction?> Create(Transaction entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(Guid id, bool isHardDelete = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Transaction?> Get(Expression<Func<Transaction, bool>>? expression = null, int size = 10, int page = 0)
    {
        throw new NotImplementedException();
    }

    public Task<Transaction?> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Transaction?> Update(Transaction entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
