using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WDA.Domain.Models.Customer;

namespace WDA.Domain.Repositories;

public class CustomerRepository : IBaseRepository<Customer>
{
    private readonly AppDbContext _dbContext;

    public CustomerRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public Task<Customer?> Create(Customer entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(Guid id, bool isHardDelete = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Customer?> Get(Expression<Func<Customer, bool>>? expression = null, int size = 10, int page = 0)
    {
        throw new NotImplementedException();
    }

    public Task<Customer?> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Customer?> Update(Customer entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
