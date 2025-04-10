using TransportManagement.Models.Finance;

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






        Task<decimal> CalculateMonthlyTotalRevenue(int year, int month);
        Task<decimal> CalculateMonthlyTotalExpenses(int year, int month);
        Task<decimal> CalculateMonthlyTotalSalaries(int year, int month);
        Task<decimal> CalculateMonthlyGrossProfit(int year, int month);
        Task<decimal> CalculateMonthlyNetProfit(int year, int month);




        Task<decimal> CalculateMonthlyTotalRevenueForUser(string employeeEmail, int year, int month);
        Task<decimal> CalculateMonthlyTotalExpensesForUser(string employeeEmail, int year, int month);
        Task<decimal> CalculateMonthlyTotalSalariesForUser(string employeeEmail, int year, int month);
        Task<decimal> CalculateMonthlyGrossProfitForUser(string employeeEmail, int year, int month);
        Task<decimal> CalculateMonthlyNetProfitForUser(string employeeEmail, int year, int month);





        Task<decimal> CalculateYearTotalRevenue(int year);
        Task<decimal> CalculateYearTotalExpenses(int year);
        Task<decimal> CalculateYearTotalSalaries(int year);
        Task<decimal> CalculateYearGrossProfit(int year);
        Task<decimal> CalculateYearNetProfit(int year);




        Task<decimal> CalculateYearTotalRevenueForUser(string employeeEmail, int year);
        Task<decimal> CalculateYearTotalExpensesForUser(string employeeEmail, int year);
        Task<decimal> CalculateYearTotalSalariesForUser(string employeeEmail, int year);
        Task<decimal> CalculateYearGrossProfitForUser(string employeeEmail, int year);
        Task<decimal> CalculateYearNetProfitForUser(string employeeEmail, int year);








        Task AddSalariesToFinance(int year, int month, int day);
        Task AddExpensesToFinance(decimal amount, string description, int year, int month, int day);
    }
}
