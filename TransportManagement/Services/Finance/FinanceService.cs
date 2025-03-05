using Microsoft.EntityFrameworkCore;
using TransportManagement.Models.Finance;
using TransportManagement.Models.FinanceReport;

namespace TransportManagement.Services.Finance
{
    public class FinanceService : IFinanceService
    {
        private readonly TransportManagementDbContext _context;

        public FinanceService(TransportManagementDbContext context)
        {
            _context = context;
        }

        public async Task AddSalariesToFinance(int year, int month, int day)
        {
            var drivers = await _context.Drivers.ToListAsync();

            foreach (var driver in drivers)
            {
                var salaryExpense = new FinanceModel
                {
                    Description = $"Pensja dla pracownika {driver.Email}",
                    DriverEmail = driver.Email,
                    Amount = driver.Salary,
                    Type = FinanceType.Salary,
                    Date = new DateTime(year, month, day)
                };

                _context.Finances.Add(salaryExpense);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> CalculateNetProfit()
        {
            var revenue = await CalculateTotalRevenue();
            var expenses = await CalculateTotalExpenses();
            decimal grossProfit = revenue - expenses;
            decimal taxRate = 0.18m;
            return grossProfit - (grossProfit * taxRate);
        }

        public async Task<decimal> CalculateGrossProfit()
        {
            var revenue = await CalculateTotalRevenue();
            var expenses = await CalculateTotalExpenses();
            return revenue - expenses;
        }

        public async Task<decimal> CalculateTotalExpenses()
        {
            return await _context.Finances.Where(f => f.Type == FinanceType.Expense).SumAsync(f => f.Amount);
        }

        public async Task<decimal> CalculateTotalRevenue()
        {
            return await _context.Finances.Where(f => f.Type == FinanceType.Revenue).SumAsync(f => f.Amount);
        }

        public async Task<decimal> CalculateTotalSalaries()
        {
            return await _context.Drivers.SumAsync(d => d.Salary);
        }

        public async Task<List<FinanceModel>> GetAllFinance()
        {
            return await _context.Finances.ToListAsync();
        }

        public async Task AddExpensesToFinance(decimal amount, string description, int year, int month, int day)
        {
            var expense = new FinanceModel
            {
                Description = description,
                Amount = amount,
                Type = FinanceType.Expense,
                Date = new DateTime(year, month, day)
            };

            _context.Finances.Add(expense);
            await _context.SaveChangesAsync();
        }

        //public async Task<List<FinanceReportModel>> GetFinanceReport()
        //{
        //    var reports = await _context.Drivers.Select(driver=>new FinanceReportModel
        //    {
        //        DriverEmail = driver.Email,
        //        Year = DateTime.Now.Year,
        //        Month = DateTime.Now.Month,
        //        TotalSalary = driver.Salary,
        //        TotalRevenue = _context.Orders.Where(o=>o.DriverEmail==driver.Email).Sum(o=>o.Revenue),
        //        TotalExpenses = driver.Salary
        //    }).ToListAsync();

        //    return reports;
        //}
    }
}
