using System.Linq.Expressions;
using WDA.Domain.Models.Thread;

namespace WDA.Domain.Repositories;

public class ReplyRepository : IBaseRepository<Reply>
{
    private readonly AppDbContext _dbContext;

    public ReplyRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public IQueryable<Reply> Get(Expression<Func<Reply, bool>>? expression = null, int size = 10, int page = 0)
    {
        throw new NotImplementedException();
    }

    public Task<Reply?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Reply?> Create(Reply entity, CancellationToken cancellationToken = default)
    {
        var res = _dbContext.Replies.Add(entity);

        return res.Entity;
    }

    public Task<Reply?> Update(Reply entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(Guid id, bool isHardDelete = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}