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
        public async Task<decimal> CalculateNetProfit()
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
            return await _context.Finances.Where(f=>f.Type == FinanceType.Revenue).SumAsync(f=>f.Amount);
        }

        public async Task<decimal> CalculateTotalSalaries()
        {
            return await _context.Drivers.SumAsync(d => d.Salary);
        }

        public async Task<List<FinanceModel>> GetAllFinance()
        {
            return await _context.Finances.ToListAsync();
        }

        public async Task<List<FinanceReportModel>> GetFinanceReport()
        {
            var reports = await _context.Drivers.Select(driver=>new FinanceReportModel
            {
                DriverEmail = driver.Email,
                Year = DateTime.Now.Year,
                Month = DateTime.Now.Month,
                TotalSalary = driver.Salary,
                TotalRevenue = _context.Orders.Where(o=>o.DriverEmail==driver.Email).Sum(o=>o.Revenue),
                TotalExpenses = driver.Salary
            }).ToListAsync();

            return reports;
        }
    }
}
