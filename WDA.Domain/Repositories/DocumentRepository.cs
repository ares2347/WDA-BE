using System.Linq.Expressions;
using WDA.Domain.Models.Document;
using WDA.Domain.Models.User;

namespace WDA.Domain.Repositories
{
    internal class DocumentRepository : IBaseRepository<Document>
    {
        private readonly AppDbContext _dbContext;

        public DocumentRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Document?> Create(Document entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Guid id, bool isHardDelete = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Document?> Get(Expression<Func<Document, bool>>? expression = null, int size = 10, int page = 0)
        {
            throw new NotImplementedException();
        }

        public Task<Document?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Document?> Update(Document entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
