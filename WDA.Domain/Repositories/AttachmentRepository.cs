using System.Linq.Expressions;
using System.Net;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using WDA.Domain.Models.Attachment;
using WDA.Shared;

namespace WDA.Domain.Repositories;

public class AttachmentRepository : IBaseRepository<Attachment>
{
    private readonly AppDbContext _dbContext;

    public AttachmentRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Attachment?> Get(Expression<Func<Attachment, bool>>? expression = null, int size = 10,
        int page = 0)
    {
        var query = _dbContext.Attachments.Where(x => !x.IsDelete);
        if (expression is not null)
        {
            query = query.Where(expression);
        }

        return query.Skip(page).Take(size);
    }

    public async Task<Attachment?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Attachments.FirstOrDefaultAsync(a => a.AttachmentId.Equals(id),
            cancellationToken: cancellationToken);
    }

    public async Task<Attachment?> Create(Attachment entity, CancellationToken cancellationToken = default)
    {
        if (entity.AttachmentId.Equals(Guid.Empty))
        {
            entity.AttachmentId = NewId.NextGuid();
        }

        var res = _dbContext.Attachments.Add(entity);

        return res.Entity;
    }

    public Task<Attachment?> Update(Attachment entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Delete(Guid id, bool isHardDelete = false, CancellationToken cancellationToken = default)
    {
        var attachment = await _dbContext.Attachments.FirstOrDefaultAsync(a => a.AttachmentId.Equals(id),
            cancellationToken: cancellationToken);

        HttpException.ThrowIfNull(attachment, HttpStatusCode.NotFound);

        attachment!.IsDelete = true;
        _dbContext.Attachments.Update(attachment);
        if (isHardDelete)
        {
            _dbContext.Remove(attachment);
        }

        return true;
    }
}