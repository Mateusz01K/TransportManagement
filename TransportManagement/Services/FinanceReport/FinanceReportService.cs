using Microsoft.EntityFrameworkCore;
using System;
using TransportManagement.Models.Finance;
using TransportManagement.Models.FinanceReport;

namespace TransportManagement.Services.FinanceReport
{
    public class FinanceReportService : IFinanceReportService
    {
        private readonly TransportManagementDbContext _context;

        public FinanceReportService(TransportManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<FinanceReportModel>> GenerateAnnualReport(int year)
        {
            //return await _context.FinanceReports.Where(r => r.Year == year).ToListAsync();
            var reports = await _context.Finances
                .Where(f => f.Date.Year == year)
                .GroupBy(f => new { f.Date.Year })
                .Select(g => new FinanceReportModel
                {
                    Year = g.Key.Year,
                    TotalRevenue = g.Where(f => f.Type == FinanceType.Revenue).Sum(f => f.Amount),
                    TotalExpenses = g.Where(f => f.Type == FinanceType.Expense).Sum(f => f.Amount),
                    TotalSalary = g.Where(f => f.Type == FinanceType.Salary).Sum(f => f.Amount)
                })
                .ToListAsync();

            return reports;
        }

        public async Task<List<FinanceReportModel>> GenerateMonthlyReport(int year, int month)
        {
            var reports = await _context.Finances
                .Where(f => f.Date.Year == year && f.Date.Month == month)
                .GroupBy(f => new { f.Date.Year, f.Date.Month })
                .Select(g => new FinanceReportModel
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalRevenue = g.Where(f => f.Type == FinanceType.Revenue).Sum(f => f.Amount),
                    TotalExpenses = g.Where(f => f.Type == FinanceType.Expense).Sum(f => f.Amount),
                    TotalSalary = g.Where(f => f.Type == FinanceType.Salary).Sum(f => f.Amount),

                    TotalProfitFromCompletedOrders = _context.FinanceReports
                        .Where(fr => fr.Year == year && fr.Month == month)
                        .Sum(fr => fr.TotalProfitFromCompletedOrders)
                })
                .ToListAsync();

            return reports;
        }

        //public async Task<List<FinanceReportModel>> GenerateMonthlyReport(int year, int month)
        //{
        //    //return await _context.FinanceReports.Where(r => r.Year == year && r.Month == month).ToListAsync();
        //    var reports = await _context.Finances
        //        .Where(f => f.Date.Year == year && f.Date.Month == month)
        //        .GroupBy(f => new { f.Date.Year, f.Date.Month })
        //        .Select(g => new FinanceReportModel
        //        {
        //            Year = g.Key.Year,
        //            Month = g.Key.Month,
        //            TotalRevenue = g.Where(f => f.Type == FinanceType.Revenue).Sum(f => f.Amount),
        //            TotalExpenses = g.Where(f => f.Type == FinanceType.Expense).Sum(f => f.Amount),
        //            TotalSalary = g.Where(f => f.Type == FinanceType.Salary).Sum(f => f.Amount)
        //        })
        //        .ToListAsync();

        //    return reports;
        //}
    }
}
