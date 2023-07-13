using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WDA.Domain.Models.Thread;
using Thread = WDA.Domain.Models.Thread.Thread;

namespace WDA.Domain.Repositories;

public class ThreadRepository : IBaseRepository<Thread>
{
    private readonly AppDbContext _dbContext;

    public ThreadRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public IQueryable<Thread> Get(Expression<Func<Thread, bool>>? expression = null, int size = 10, int page = 0)
    {
        var query = _dbContext.Threads.Where(x => !x.IsDelete);
        if (expression is not null)
        {
            query = query.Where(expression);
        }

        return query.Skip(page).Take(size);
    }

    public Task<Thread?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Threads.Include(x => x.Replies)
            .Include(x => x.CreatedBy)
            .Include(x => x.ModifiedBy)
            .FirstOrDefaultAsync(x => x.ThreadId.Equals(id) && !x.IsDelete, cancellationToken);
    }

    public async Task<Thread?> Create(Thread entity, CancellationToken cancellationToken = default)
    {
        var res = _dbContext.Threads.Add(entity);

        return res.Entity;
    }
    
    public Task<Thread?> Update(Thread entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(Guid id, bool isHardDelete = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}