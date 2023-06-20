using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using WDA.Domain.Models.Transaction;
using WDA.Domain.Models.User;
using WDA.Shared;

namespace WDA.Domain.Repositories;

public class TransactionRepository : IBaseRepository<Transaction>
{
    private readonly AppDbContext _dbContext;

    public TransactionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Transaction?> Create(Transaction entity, CancellationToken cancellationToken = default)
    {
        var res = _dbContext.Transactions.Add(entity);
        return res.Entity;
    }

    public async Task<bool> Delete(Guid id, bool isHardDelete = false, CancellationToken cancellationToken = default)
    {
        var transaction = await _dbContext.Transactions.FirstOrDefaultAsync(x => x.TransactionId.Equals(id) && !x.IsDelete, cancellationToken);
        if (transaction is null) return false;
        if (isHardDelete)
        {
            _dbContext.Transactions.Remove(transaction);
            return true;
        }
        transaction.IsDelete = true;
        _dbContext.Transactions.Update(transaction);
        return true;
    }

    public IQueryable<Transaction?> Get(Expression<Func<Transaction, bool>>? expression = null, int size = 10, int page = 0)
    {
        if (expression is null)
        {
            return _dbContext.Transactions
                .Include(x => x.CreatedBy)
                .Include(x => x.ModifiedBy)
                .Take(size)
                .Skip(page * size)
                .OrderBy(x => x.ModifiedAt)
                .Reverse()
                .Where(x => !x.IsDelete);
        }
        return _dbContext.Transactions
                .Include(x => x.CreatedBy)
                .Include(x => x.ModifiedBy)
                .Take(size)
                .Skip(page * size)
                .OrderBy(x => x.ModifiedAt)
                .Reverse()
                .Where(x => !x.IsDelete)
                .Where(expression);
    }

    public async Task<Transaction?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var transaction = await _dbContext.Transactions.FirstOrDefaultAsync(x => x.TransactionId.Equals(id) && !x.IsDelete, cancellationToken);
        return transaction;
    }

    public Task<Transaction?> Update(Transaction entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
