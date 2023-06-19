using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WDA.Domain.Models.Document;

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

        public Task<Document?> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Document?> Update(Document entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
