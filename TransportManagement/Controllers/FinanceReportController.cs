using DinkToPdf.Contracts;
using IronPdf;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using TransportManagement.Models.FinanceReport;
using TransportManagement.Services.FinanceReport;
using Microsoft.EntityFrameworkCore;
using TransportManagement.Models.Finance;
using TransportManagement.Models.Orders;
using TransportManagement.Services.Finance;

namespace TransportManagement.Controllers
{
    public class FinanceReportController : Controller
    {
        private readonly IFinanceService _financeService;
        private readonly IConverter _pdfConverter;

        public FinanceReportController(IFinanceService financeService, IConverter pdfConverter)
        {
            _financeService = financeService;
            _pdfConverter = pdfConverter;
        }

        public IActionResult GenerateReport()
        {
            return View();
        }

        public async Task<IActionResult> DownloadMonthlyReport(string employeeEmail, int year, int month)
        {
            var revenue = await _financeService.CalculateMonthlyTotalRevenueForUser(employeeEmail, year, month);
            var expenses = await _financeService.CalculateMonthlyTotalExpensesForUser(employeeEmail, year, month);
            var salary = await _financeService.CalculateMonthlyTotalSalariesForUser(employeeEmail, year, month);
            var grossProfit = await _financeService.CalculateMonthlyGrossProfitForUser(employeeEmail, year, month);
            var netProfit = await _financeService.CalculateMonthlyNetProfitForUser(employeeEmail, year, month);

            var reports = new List<FinanceReportModel>
            {
                new FinanceReportModel
                {
                    EmployeeEmail = employeeEmail,
                    Year = year,
                    Month = month,
                    TotalRevenue = revenue,
                    TotalExpenses = expenses,
                    TotalSalary = salary,
                    NetProfit = netProfit
                }
            };

            string htmlContent = GenerateFinanceReportHtml(reports, year, month);

            var pdf = ConvertHtmlToPdf(htmlContent);

            return File(pdf, "application/pdf", $"Raport_{employeeEmail}_{year}_{month}.pdf");
        }

        public async Task<IActionResult> DownloadAnnualReport(string employeeEmail, int year)
        {

            var reports = new List<FinanceReportModel>();
            for (int month = 1; month <= 12; month++)
            {
                //var revenue = await _financeService.CalculateYearTotalRevenueForUser(employeeEmail, year);
                //var expenses = await _financeService.CalculateYearTotalExpensesForUser(employeeEmail, year);
                //var salary = await _financeService.CalculateYearTotalSalariesForUser(employeeEmail, year);
                //var grossProfit = await _financeService.CalculateYearGrossProfitForUser(employeeEmail, year);
                //var netProfit = await _financeService.CalculateYearNetProfitForUser(employeeEmail, year);
                var revenue = await _financeService.CalculateMonthlyTotalRevenueForUser(employeeEmail, year, month);
                var expenses = await _financeService.CalculateMonthlyTotalExpensesForUser(employeeEmail, year, month);
                var salary = await _financeService.CalculateMonthlyTotalSalariesForUser(employeeEmail, year, month);
                var grossProfit = await _financeService.CalculateMonthlyGrossProfitForUser(employeeEmail, year, month);
                var netProfit = await _financeService.CalculateMonthlyNetProfitForUser(employeeEmail, year, month);


                //var reports = new List<FinanceReportModel>
                reports.Add(new FinanceReportModel
                {
                    //    new FinanceReportModel
                    //    {
                    EmployeeEmail = employeeEmail,
                    Year = year,
                    Month = month,
                    TotalRevenue = revenue,
                    TotalExpenses = expenses,
                    TotalSalary = salary,
                    NetProfit = netProfit
                    //}
                });
            }
            string htmlContent = GenerateAnnualFinanceReportHtml(reports, year);

            var pdf = ConvertHtmlToPdf(htmlContent);

            return File(pdf, "application/pdf", $"Raport_{employeeEmail}_{year}.pdf");
        }

        private byte[] ConvertHtmlToPdf(string htmlContent)
        {
            var renderer = new ChromePdfRenderer();
            var pdf = renderer.RenderHtmlAsPdf(htmlContent);
            return pdf.BinaryData;
        }

        //public async Task<IActionResult> DownloadMonthlyReport(string employeeEmail, int year, int month)
        //{
        //    var reports = await _financeReportService.GenerateMonthlyReport(year, month, employeeEmail);
        //    string htmlContent = GenerateFinanceReportHtml(reports, year, month);

        //    var pdf = ConvertHtmlToPdf(htmlContent);

        //    return File(pdf, "application/pdf", $"Raport_{employeeEmail}_{year}_{month}.pdf");
        //}

        //public async Task<IActionResult> DownloadAnnualReport(string employeeEmail, int year)
        //{
        //    var reports = await _financeReportService.GenerateAnnualReport(year, employeeEmail);
        //    string htmlContent = GenerateFinanceReportHtml(reports, year, 0);

        //    var pdf = ConvertHtmlToPdf(htmlContent);

        //    return File(pdf, "application/pdf", $"Raport_{employeeEmail}_{year}.pdf");
        //}

        private string GenerateFinanceReportHtml(List<FinanceReportModel> reports, int year, int month = 0)
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
    <h2>Raport finansowy dla pracownika - {year}{(month != 0 ? $"/{month}" : "")}</h2>
    <table>
        <tr>
            <th>Email pracownika</th>
            <th>Rok</th>
            <th>Miesiąc</th>
            <th>Przychód</th>
            <th>Wydatki</th>
            <th>Wynagrodzenie</th>
            <th>Zysk netto</th>
        </tr>");

            foreach (var report in reports)
            {
                string totalRevenue = report.TotalRevenue.ToString("0.00");
                string totalExpenses = report.TotalExpenses.ToString("0.00");
                string totalSalary = report.TotalSalary.ToString("0.00");
                string netProfit = report.NetProfit.ToString("0.00");

                html.Append($@"
    <tr>
        <td>{report.EmployeeEmail}</td>
        <td>{report.Year}</td>
        <td>{(report.Month != 0 ? report.Month.ToString() : "Cały rok")}</td>
        <td>{totalRevenue} zł</td>
        <td>{totalExpenses} zł</td>
        <td>{totalSalary} zł</td>
        <td>{netProfit} zł</td>
    </tr>");
            }

            html.Append("</table></body></html>");

            return html.ToString();
        }


        private string GenerateAnnualFinanceReportHtml(List<FinanceReportModel> reports, int year, int month = 0)
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
    <h2>Raport finansowy dla pracownika - {year}{(month != 0 ? $"/{month}" : "")}</h2>
    <table>
        <tr>
            <th>Email pracownika</th>
            <th>Rok</th>
            <th>Miesiąc</th>
            <th>Przychód</th>
            <th>Wydatki</th>
            <th>Wynagrodzenie</th>
            <th>Zysk netto</th>
        </tr>");

            foreach (var report in reports)
            {
                string totalRevenue = report.TotalRevenue.ToString("0.00");
                string totalExpenses = report.TotalExpenses.ToString("0.00");
                string totalSalary = report.TotalSalary.ToString("0.00");
                string netProfit = report.NetProfit.ToString("0.00");

                html.Append($@"
    <tr>
        <td>{report.EmployeeEmail}</td>
        <td>{report.Year}</td>
        <td>{report.Month}</td>
        <td>{totalRevenue} zł</td>
        <td>{totalExpenses} zł</td>
        <td>{totalSalary} zł</td>
        <td>{netProfit} zł</td>
    </tr>");
            }

            html.Append("</table></body></html>");

            return html.ToString();
        }

        //[HttpGet]
        //public async Task<IActionResult> FinanceMonthlyReport(int year, int month)
        //{
        //    var reports = await _financeReportService.GenerateMonthlyReport(year, month);
        //    var viewModel = new FinanceReportViewModel
        //    {
        //        FinanceReports = reports
        //    };
        //    return View(viewModel);
        //}

        //[HttpGet]
        //public async Task<IActionResult> FinanceAnnualReport(int year)
        //{
        //    var reports = await _financeReportService.GenerateAnnualReport(year);
        //    var viewModel = new FinanceReportViewModel
        //    {
        //        FinanceReports = reports
        //    };
        //    return View(viewModel);
        //}
    }
}
