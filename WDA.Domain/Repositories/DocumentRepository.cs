using System.Linq.Expressions;
using System.Net;
using Microsoft.EntityFrameworkCore;
using WDA.Domain.Models.Document;
using WDA.Domain.Models.User;
using WDA.Shared;

namespace WDA.Domain.Repositories
{
    internal class DocumentRepository : IBaseRepository<Document>
    {
        private readonly AppDbContext _dbContext;

        public DocumentRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Document?> Create(Document entity, CancellationToken cancellationToken = default)
        {
            var res = _dbContext.Documents.Add(entity);

            return res.Entity;
        }

        public async Task<bool> Delete(Guid id, bool isHardDelete = false,
            CancellationToken cancellationToken = default)
        {
            var document = await _dbContext.Documents.FirstOrDefaultAsync(a => a.DocumentId.Equals(id),
                cancellationToken: cancellationToken);

            HttpException.ThrowIfNull(document, HttpStatusCode.NotFound);

            document!.IsDelete = true;
            _dbContext.Documents.Update(document);
            if (isHardDelete)
            {
                _dbContext.Remove(document);
            }

            return true;
        }

        public IQueryable<Document?> Get(Expression<Func<Document, bool>>? expression = null, int size = 10,
            int page = 0)
        {
            var query = _dbContext.Documents.Where(x => !x.IsDelete);
            if (expression is not null)
            {
                query = query.Where(expression);
            }

            return query.Skip(page).Take(size);
        }

        public Task<Document?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            return _dbContext.Documents.Include(x => x.Attachments)
                .Include(x => x.CreatedBy)
                .Include(x => x.ModifiedBy)
                .FirstOrDefaultAsync(x => x.DocumentId.Equals(id) && !x.IsDelete, cancellationToken);
        }

        public async Task<Document?> Update(Document entity, CancellationToken cancellationToken = default)
        {
            var res = _dbContext.Documents.Update(entity);

            return res.Entity;
        }
    }
}