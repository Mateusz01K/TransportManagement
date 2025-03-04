using TransportManagement.Models.Finance;
using TransportManagement.Models.FinanceReport;

namespace TransportManagement.Services.Finance
{
    public interface IFinanceService
    {
        Task<decimal> CalculateTotalRevenue();
        Task<decimal> CalculateTotalExpenses();
        Task<decimal> CalculateNetProfit();
        Task<decimal> CalculateTotalSalaries();
        Task<List<FinanceModel>> GetAllFinance();
        Task<List<FinanceReportModel>> GetFinanceReport();
    }
}
