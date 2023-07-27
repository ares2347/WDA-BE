using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WDA.Domain.Enums;
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
        ticket.LastModified = DateTimeOffset.UtcNow;
        var res = _dbContext.CustomerTickets.Update(ticket);
        return res.Entity;
    }

    public IQueryable<EmployeeTicket> GetEmployeeTickets(Expression<Func<EmployeeTicket, bool>>? expression,
        int? size, int? page)
    {
        var query = _dbContext.EmployeeTickets
            .Include(x => x.Requestor)
            .Include(x => x.Resolver).AsQueryable();
        if (expression is not null)
        {
            query = query.Where(expression);
        }

        if (size is null) return query;
        query = page is not null ? query.Skip(page.Value * size.Value).Take(size.Value) : query.Take(size.Value);


        return query;
    }

    public EmployeeTicket CreateEmployeeTicket(EmployeeTicket ticket)
    {
        var res = _dbContext.EmployeeTickets.Add(ticket);
        return res.Entity;
    }

    public EmployeeTicket UpdateEmployeeTicket(EmployeeTicket ticket)
    {
        ticket.LastModified = DateTimeOffset.UtcNow;
        var res = _dbContext.EmployeeTickets.Update(ticket);
        return res.Entity;
    }

    public async Task CloseTicketsAfter3Days()
    {
        var customerTickets = await _dbContext.CustomerTickets
            .Where(x => x.Status == TicketStatus.Done && x.LastModified.AddDays(3) <= DateTimeOffset.UtcNow)
            .ToListAsync();
        var employeeTickets = await _dbContext.EmployeeTickets
            .Where(x => x.Status == TicketStatus.Done && x.LastModified.AddDays(3) <= DateTimeOffset.UtcNow)
            .ToListAsync();
        foreach (var ticket in customerTickets)
        {
            ticket.Status = TicketStatus.Closed;
            _dbContext.CustomerTickets.Update(ticket);
        }

        foreach (var ticket in employeeTickets)
        {
            ticket.Status = TicketStatus.Closed;
            _dbContext.EmployeeTickets.Update(ticket);
        }

        await _dbContext.SaveChangesAsync();
    }
}