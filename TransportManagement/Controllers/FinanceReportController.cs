using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using TransportManagement.Models.FinanceReport;
using TransportManagement.Services.FinanceReport;
using Microsoft.EntityFrameworkCore;
using TransportManagement.Models.Finance;
using TransportManagement.Models.Orders;

namespace TransportManagement.Controllers
{
    public class FinanceReportController : Controller
    {
        private readonly IFinanceReportService _financeReportService;
        private readonly IConverter _pdfConverter;
        private readonly TransportManagementDbContext _context;

        public FinanceReportController(IFinanceReportService financeReportService, IConverter pdfConverter, TransportManagementDbContext context)
        {
            _financeReportService = financeReportService;
            _pdfConverter = pdfConverter;
            _context = context;
        }

        public async Task<IActionResult> DownloadMonthlyReport(string driverEmail, int year, int month)
        {
            var reports = await _financeReportService.GenerateMonthlyReport(year, month);
            string htmlContent = GenerateFinanceReportHtml(reports, year, month);

            var pdf = _pdfConverter.Convert(new HtmlToPdfDocument
            {
                GlobalSettings = { PaperSize = PaperKind.A4, Orientation = Orientation.Portrait },
                Objects = { new ObjectSettings { HtmlContent = htmlContent, WebSettings = { DefaultEncoding = "utf-8" } } }
            });

            return File(pdf, "application/pdf", $"Raport_{year}_{month}.pdf");
        }

        private string GenerateFinanceReportHtml(List<FinanceReportModel> reports, int year, int month)
        {
            StringBuilder html = new StringBuilder();

            html.Append($@"
    <html>
    <head>
        <style>
            body {{ font-family: Arial, sans-serif; }}
            h2 {{ text-align: center; }}
            table {{ width: 100%; border-collapse: collapse; }}
            th, td {{ border: 1px solid black; padding: 8px; text-align: center; }}
            th {{ background-color: #f2f2f2; }}
        </style>
    </head>
    <body>
        <h2>Raport finansowy dla kierowcy - {year}/{month}</h2>
        <table>
            <tr>
                <th>Data</th>
                <th>Przychód</th>
                <th>Wydatki</th>
                <th>Wypłaty</th>
                <th>Zysk z zakończonych zleceń</th>
                <th>Zysk</th>
            </tr>");

            foreach (var report in reports)
            {
                var formattedMonth = report.Month.ToString("D2");

                html.Append($@"
        <tr>
            <td>{report.Year}/{formattedMonth}</td>
            <td>{report.TotalRevenue} zł</td>
            <td>{report.TotalExpenses} zł</td>
            <td>{report.TotalSalary} zł</td>
            <td>{report.TotalProfitFromCompletedOrders} zł</td>
            <td>{(report.TotalRevenue - report.TotalExpenses - report.TotalSalary + report.TotalProfitFromCompletedOrders)} zł</td>
        </tr>");
            }

            html.Append("</table></body></html>");

            return html.ToString();
        }

        public async Task<IActionResult> FinanceMonthlyReport(int year, int month)
        {
            var reports = await _financeReportService.GenerateMonthlyReport(year, month);
            var viewModel = new FinanceReportViewModel
            {
                FinanceReports = reports
            };
            return View(viewModel);
        }

        public async Task<IActionResult> FinanceAnnualReport(int year)
        {
            var reports = await _financeReportService.GenerateAnnualReport(year);
            var viewModel = new FinanceReportViewModel
            {
                FinanceReports = reports
            };
            return View(viewModel);
        }
    }
}
