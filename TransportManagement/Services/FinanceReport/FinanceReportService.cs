using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using TransportManagement.Models.Finance;
using TransportManagement.Models.FinanceReport;
using TransportManagement.Models.User;

namespace TransportManagement.Services.FinanceReport
{
    public class FinanceReportService : IFinanceReportService
    {
        private readonly TransportManagementDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FinanceReportService(TransportManagementDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
                    TotalRevenue = g.Where(f => f.Type == FinanceType.Przychód).Sum(f => f.Amount),
                    TotalExpenses = g.Where(f => f.Type == FinanceType.Wydatek).Sum(f => f.Amount),
                    TotalSalary = g.Where(f => f.Type == FinanceType.Pensja).Sum(f => f.Amount),
                    NetProfit = g.Where(f => f.Type == FinanceType.Przychód).Sum(f => f.Amount) -
                                g.Where(f => f.Type == FinanceType.Wydatek).Sum(f => f.Amount) -
                                g.Where(f => f.Type == FinanceType.Pensja).Sum(f => f.Amount)
                })
                .ToListAsync();

            return reports;
        }

        public async Task<List<FinanceReportModel>> GenerateAnnualReport(int year, string employeeEmail)
        {
            var query = _context.Finances
            .Where(f => f.Date.Year == year);


            if (!string.IsNullOrEmpty(employeeEmail))
            {
                query = query.Where(f => f.EmployeeEmail == employeeEmail);
            }

            var reports = await query
                .GroupBy(f => new { f.EmployeeEmail })
                .Select(g => new FinanceReportModel
                {
                    EmployeeEmail = g.Key.EmployeeEmail,
                    Year = year,
                    TotalRevenue = g.Where(f => f.Type == FinanceType.Przychód).Sum(f => f.Amount),
                    TotalExpenses = g.Where(f => f.Type == FinanceType.Wydatek).Sum(f => f.Amount),
                    TotalSalary = g.Where(f => f.Type == FinanceType.Pensja).Sum(f => f.Amount),
                    NetProfit = g.Where(f => f.Type == FinanceType.Przychód).Sum(f => f.Amount) -
                                g.Where(f => f.Type == FinanceType.Wydatek).Sum(f => f.Amount) -
                                g.Where(f => f.Type == FinanceType.Pensja).Sum(f => f.Amount)
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
                    TotalRevenue = g.Where(f => f.Type == FinanceType.Przychód).Sum(f => f.Amount),
                    TotalExpenses = g.Where(f => f.Type == FinanceType.Wydatek).Sum(f => f.Amount),
                    TotalSalary = g.Where(f => f.Type == FinanceType.Pensja).Sum(f => f.Amount),
                    NetProfit = g.Where(f => f.Type == FinanceType.Przychód).Sum(f => f.Amount) -
                                g.Where(f => f.Type == FinanceType.Wydatek).Sum(f => f.Amount) -
                                g.Where(f => f.Type == FinanceType.Pensja).Sum(f => f.Amount),

                    TotalProfitFromCompletedOrders = _context.FinanceReports
                        .Where(fr => fr.Year == year && fr.Month == month)
                        .Sum(fr => fr.TotalProfitFromCompletedOrders)
                })
                .ToListAsync();

            return reports;
        }

        public async Task<List<FinanceReportModel>> GenerateMonthlyReport(int year, int month, string employeeEmail)
        {
            var query = _context.Finances
            .Where(f => f.Date.Year == year && f.Date.Month == month);


            if (!string.IsNullOrEmpty(employeeEmail))
            {
                query = query.Where(f => f.EmployeeEmail == employeeEmail);
            }

            var reports = await query
                .GroupBy(f => new { f.EmployeeEmail, f.Date.Year, f.Date.Month })
                .Select(g => new FinanceReportModel
                {
                    EmployeeEmail = g.Key.EmployeeEmail,
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalRevenue = g.Where(f => f.Type == FinanceType.Przychód).Sum(f => f.Amount),
                    TotalExpenses = g.Where(f => f.Type == FinanceType.Wydatek).Sum(f => f.Amount),
                    TotalSalary = g.Where(f => f.Type == FinanceType.Pensja).Sum(f => f.Amount),
                    NetProfit = g.Where(f => f.Type == FinanceType.Przychód).Sum(f => f.Amount) -
                                g.Where(f => f.Type == FinanceType.Wydatek).Sum(f => f.Amount) +
                                g.Where(f => f.Type == FinanceType.Pensja).Sum(f => f.Amount)
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
