using Microsoft.AspNetCore.Mvc;
using TransportManagement.Services.FinanceReport;

namespace TransportManagement.Controllers
{
    public class FinanceReportController : Controller
    {
        private readonly IFinanceReportService _financeReportService;

        public FinanceReportController(IFinanceReportService financeReportService)
        {
            _financeReportService = financeReportService;
        }

        public async Task<IActionResult> FinanceMonthlyReport(int year, int month)
        {
            var reports = await _financeReportService.GenerateMonthlyReport(year, month);
            return View(reports);
        }

        public async Task<IActionResult> FinanceAnnualReport(int year)
        {
            var reports = await _financeReportService.GenerateAnnualReport(year);
            return View(reports);
        }
    }
}
