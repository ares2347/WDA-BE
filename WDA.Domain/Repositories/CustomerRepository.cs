using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WDA.Domain.Models.Customer;
using WDA.Domain.Models.User;
using WDA.Shared;

namespace WDA.Domain.Repositories;

public class CustomerRepository : IBaseRepository<Customer>
{
    private readonly AppDbContext _dbContext;

    public CustomerRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Customer?> Create(Customer entity, CancellationToken cancellationToken = default)
    {
        var existedCustomer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Email.Equals(entity.Email) && !x.IsDelete, cancellationToken);
        if (existedCustomer is not null)
        {
            throw new HttpException("Customer existed!", System.Net.HttpStatusCode.BadRequest);
        }
        var res = _dbContext.Customers.Add(entity);
        return res.Entity;

    }

    public async Task<bool> Delete(Guid id, bool isHardDelete = false, CancellationToken cancellationToken = default)
    {
        var existedCustomer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.CustomerId.Equals(id) && !x.IsDelete, cancellationToken);
        if (existedCustomer is null) return false; 
        if (isHardDelete)
        {
            _dbContext.Customers.Remove(existedCustomer);
            return true;
        }
        existedCustomer.IsDelete = true;
        _dbContext.Customers.Update(existedCustomer);
        return true;
    }

    public IQueryable<Customer?> Get(Expression<Func<Customer, bool>>? expression = null, int size = 10, int page = 0)
    {
        if (expression is null)
        {
            return _dbContext.Customers
                .Include(x => x.CreatedBy)
                .Include(x => x.ModifiedBy)
                .Take(size)
                .Skip(page * size)
                .OrderBy(x => x.ModifiedAt)
                .Reverse()
                .Where(x => !x.IsDelete);
        }
        return _dbContext.Customers
                .Include(x => x.CreatedBy)
                .Include(x => x.ModifiedBy)
                .Take(size)
                .Skip(page * size)
                .OrderBy(x => x.ModifiedAt)
                .Reverse()
                .Where(x => !x.IsDelete)
                .Where(expression);
    }

    public Task<Customer?> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Customer?> Update(Customer entity, CancellationToken cancellationToken = default)
    {
        var existedCustomer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.CustomerId.Equals(entity.CustomerId), cancellationToken);
        if (existedCustomer is null)
        {
            return null;
        }
        existedCustomer.Address = entity.Address;
        existedCustomer.Telephone = entity.Telephone;
        existedCustomer.Email = entity.Email;
        existedCustomer.Name = entity.Name;
        existedCustomer.ModifiedAt = entity.ModifiedAt;
        existedCustomer.ModifiedBy = entity.ModifiedBy;
        _dbContext.Customers.Update(existedCustomer);
        return existedCustomer;
    }
}