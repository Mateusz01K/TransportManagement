using TransportManagement.Models.Finance;
using TransportManagement.Models.FinanceReport;

namespace TransportManagement.Services.Finance
{
    public interface IFinanceService
    {
        Task<decimal> CalculateTotalRevenue();
        Task<decimal> CalculateTotalExpenses();
        Task<decimal> CalculateNetProfit();
        Task<decimal> CalculateGrossProfit();
        Task<decimal> CalculateTotalSalaries();
        Task<List<FinanceModel>> GetAllFinance();
        Task AddSalariesToFinance(int year, int month, int day);
        Task AddExpensesToFinance(decimal amount, string description, int year, int month, int day);
        //Task<List<FinanceReportModel>> GetFinanceReport();
    }
}
