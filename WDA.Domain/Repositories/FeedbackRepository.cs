using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WDA.Domain.Models.Feedback;

namespace WDA.Domain.Repositories;

public class FeedbackRepository : IBaseRepository<Feedback>
{
    private readonly AppDbContext _dbContext;

    public FeedbackRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Feedback> Get(Expression<Func<Feedback, bool>>? expression = null, int size = 10, int page = 0)
    {
        if (expression is null)
        {
            return _dbContext.Feedbacks
                .Include(x => x.Customer)
                .Include(x => x.CreatedBy)
                .Include(x => x.ModifiedBy)
                .Take(size)
                .Skip(page * size)
                .OrderBy(x => x.ModifiedAt)
                .Reverse()
                .Where(x => !x.IsDelete);
        }

        return _dbContext.Feedbacks
            .Include(x => x.Customer)
            .Include(x => x.CreatedBy)
            .Include(x => x.ModifiedBy)
            .Take(size)
            .Skip(page * size)
            .OrderBy(x => x.ModifiedAt)
            .Reverse()
            .Where(x => !x.IsDelete)
            .Where(expression);
    }

    public async Task<Feedback?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var feedback = await Get(x => x.FeedbackId.Equals(id)).FirstOrDefaultAsync(cancellationToken);
        return feedback;
    }

    public async Task<Feedback?> Create(Feedback entity, CancellationToken cancellationToken = default)
    {
        var res = _dbContext.Feedbacks.Add(entity);
        return res.Entity;
    }

    public Task<Feedback?> Update(Feedback entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(Guid id, bool isHardDelete = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}