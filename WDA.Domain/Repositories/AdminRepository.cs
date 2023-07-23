using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WDA.Domain.Enums;
using WDA.Domain.Models.Admin;
using WDA.Domain.Models.User;

namespace WDA.Domain.Repositories;

public class AdminRepository
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<User> _userManager;

    public AdminRepository(AppDbContext dbContext, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<Analysis> GetAnalysis(CancellationToken _)
    {
        var monthIncome = await _dbContext.Transactions
            .Where(x => x.CreatedAt.Value.Month == DateTimeOffset.UtcNow.Month)
            .SumAsync(x => x.Total, cancellationToken: _);
        var pastIncome = await _dbContext.Transactions
            .Where(x => x.CreatedAt.Value.AddMonths(1).Month == DateTimeOffset.UtcNow.Month)
            .SumAsync(x => x.Total, cancellationToken: _); 
        var totalIncome = await _dbContext.Transactions.SumAsync(x => x.Total, cancellationToken: _);
        
        var monthTransaction = await _dbContext.Transactions
            .Where(x => x.CreatedAt.Value.Month == DateTimeOffset.UtcNow.Month)
            .CountAsync(cancellationToken: _);
        var pastTransaction = await _dbContext.Transactions
            .Where(x => x.CreatedAt.Value.AddMonths(1).Month == DateTimeOffset.UtcNow.Month)
            .CountAsync(cancellationToken: _);
        var totalTransaction = await _dbContext.Transactions
            .Where(x => x.CreatedAt.Value.Year == DateTimeOffset.UtcNow.Year).CountAsync(_);
        var monthCustomers = await _dbContext.Transactions
            .Where(x => x.CreatedAt.Value.Month == DateTimeOffset.UtcNow.Month).Select(x => x.Customer).Distinct()
            .CountAsync(_);
        var pastCustomers = await _dbContext.Transactions
            .Where(x => x.CreatedAt.Value.AddMonths(1).Month == DateTimeOffset.UtcNow.Month).Select(x => x.Customer).Distinct()
            .CountAsync(_);
        var totalManager = await _userManager.GetUsersInRoleAsync(RoleName.HrManager);
        var totalEmployee = await _userManager.GetUsersInRoleAsync(RoleName.SaleManager);
        
        var monthlyTransactions = await _dbContext.Transactions
            .Where(x => x.CreatedAt.Value.Year == DateTimeOffset.UtcNow.Year)
            .GroupBy(x => x.CreatedAt.Value.Month)
            .Select(x => new CountByMonth { Month = x.Key, Total = x.Count() }).ToListAsync();
        var monthlyIncomes = await _dbContext.Transactions
            .Where(x => x.CreatedAt.Value.Year == DateTimeOffset.UtcNow.Year)
            .GroupBy(x => x.CreatedAt.Value.Month)
            .Select(x => new CountByMonth { Month = x.Key, Total = x.Sum(x => x.Total) }).ToListAsync();
       
        return new Analysis
        {
            MonthIncome = monthIncome,
            PastIncome = pastIncome,
            TotalIncome = totalIncome,
            MonthTransaction = monthTransaction,
            PastTransaction = pastTransaction,
            TotalTransaction = totalTransaction,
            TransactionByMonths = monthlyTransactions,
            IncomeByMonths = monthlyIncomes,
            MonthCustomer = monthCustomers,
            PastCustomer = pastCustomers,
            TotalStaff = totalManager.Count()+totalEmployee.Count()
        };
    }
}