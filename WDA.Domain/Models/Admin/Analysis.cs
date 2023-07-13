namespace WDA.Domain.Models.Admin;

public class Analysis
{
    public decimal MonthIncome { get; set; }
    public decimal PastIncome { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal MonthTransaction { get; set; }
    public decimal PastTransaction { get; set; }
    public int TotalTransaction { get; set; }
    public List<CountByMonth> IncomeByMonths { get; set; } = new();
    public List<CountByMonth> TransactionByMonths { get; set; } = new();
    public int MonthCustomer { get; set; }
    public int PastCustomer { get; set; }
    public int TotalStaff { get; set; }
}

public class CountByMonth
{
    public int Month { get; set; }
    public decimal Total { get; set; }
}