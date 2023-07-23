using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WDA.Domain.Models.Ticket;
using WDA.Domain.Models.User;
using WDA.Shared;

namespace WDA.Domain.Repositories;

public class TicketRepository
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<User> _userManager;

    public TicketRepository(AppDbContext dbContext, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public IQueryable<CustomerTicket> GetCustomerTickets(Expression<Func<CustomerTicket, bool>>? expression,
        int? size, int? page)
    {
        var query = _dbContext.CustomerTickets
            .Include(x => x.Requestor)
            .Include(x => x.Resolver).AsQueryable();
        if (expression is not null)
        {
            query = query.Where(expression);
        }

        if (size is not null)
        {
            if (page is not null)
            {
                query = query.Skip(page.Value * size.Value).Take(size.Value);
            }
            else
            {
                query = query.Take(size.Value);
            }
        }
        
        
        return query;
    }

    public CustomerTicket CreateCustomerTicket(CustomerTicket ticket)
    {
        var res = _dbContext.CustomerTickets.Add(ticket);
        return res.Entity;
    }    
    
    public CustomerTicket UpdateCustomerTicket(CustomerTicket ticket)
    {
        var res = _dbContext.CustomerTickets.Update(ticket);
        return res.Entity;
    }
}