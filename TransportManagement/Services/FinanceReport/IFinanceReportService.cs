using TransportManagement.Models.FinanceReport;

namespace TransportManagement.Services.FinanceReport
{
    public interface IFinanceReportService
    {
        Task<List<FinanceReportModel>> GenerateMonthlyReport(int year, int month);
        Task<List<FinanceReportModel>> GenerateMonthlyReport(int year, int month, string employeeEmail);
        Task<List<FinanceReportModel>> GenerateAnnualReport(int year);
        Task<List<FinanceReportModel>> GenerateAnnualReport(int year, string employeeEmail);
    }
}
