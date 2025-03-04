using Microsoft.EntityFrameworkCore;
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
            return await _context.FinanceReports.Where(r => r.Year == year).ToListAsync();
        }

        public async Task<List<FinanceReportModel>> GenerateMonthlyReport(int year, int month)
        {
            return await _context.FinanceReports.Where(r => r.Year == year && r.Month == month).ToListAsync();
        }
    }
}
