using Microsoft.EntityFrameworkCore;
using TransportManagement.Models.Finance;
using TransportManagement.Models.FinanceReport;

namespace TransportManagement.Services.Finance
{
    public class FinanceService : IFinanceService
    {
        private readonly TransportManagementDbContext _context;

        public FinanceService(TransportManagementDbContext context)
        {
            _context = context;
        }

        public async Task AddSalariesToFinance(int year, int month, int day)
        {
            var drivers = await _context.Drivers.ToListAsync();

            foreach (var driver in drivers)
            {
                var salaryExpense = new FinanceModel
                {
                    Description = $"Pensja",
                    EmployeeEmail = driver.Email,
                    Amount = driver.Salary,
                    Type = FinanceType.Pensja,
                    Date = new DateTime(year, month, day)
                };

                _context.Finances.Add(salaryExpense);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> CalculateNetProfit()
        {
            var revenue = await CalculateTotalRevenue();
            var expenses = await CalculateTotalExpenses();
            decimal grossProfit = revenue - expenses;
            decimal taxRate = 0.18m;
            return grossProfit - (grossProfit * taxRate);
        }

        public async Task<decimal> CalculateGrossProfit()
        {
            var revenue = await CalculateTotalRevenue();
            var expenses = await CalculateTotalExpenses();
            return revenue - expenses;
        }

        public async Task<decimal> CalculateTotalExpenses()
        {
            return await _context.Finances.Where(f => f.Type == FinanceType.Wydatek || f.Type == FinanceType.Pensja).SumAsync(f => f.Amount);
        }

        public async Task<decimal> CalculateTotalRevenue()
        {
            return await _context.Finances.Where(f => f.Type == FinanceType.Przychód).SumAsync(f => f.Amount);
        }

        public async Task<decimal> CalculateTotalSalaries()
        {
            return await _context.Drivers.SumAsync(d => d.Salary);
        }

        public async Task<List<FinanceModel>> GetAllFinance()
        {
            return await _context.Finances.ToListAsync();
        }

        public async Task AddExpensesToFinance(decimal amount, string description, int year, int month, int day)
        {
            var expense = new FinanceModel
            {
                Description = description,
                Amount = amount,
                Type = FinanceType.Wydatek,
                Date = new DateTime(year, month, day)
            };

            _context.Finances.Add(expense);
            await _context.SaveChangesAsync();
        }

        //public async Task<List<FinanceReportModel>> GetFinanceReport()
        //{
        //    var reports = await _context.Drivers.Select(driver=>new FinanceReportModel
        //    {
        //        DriverEmail = driver.Email,
        //        Year = DateTime.Now.Year,
        //        Month = DateTime.Now.Month,
        //        TotalSalary = driver.Salary,
        //        TotalRevenue = _context.Orders.Where(o=>o.DriverEmail==driver.Email).Sum(o=>o.Revenue),
        //        TotalExpenses = driver.Salary
        //    }).ToListAsync();

        //    return reports;
        //}





        public async Task<decimal> CalculateMonthlyTotalRevenue(int year, int month)
        {
            return await _context.Finances
                .Where(f => f.Type == FinanceType.Przychód && f.Date.Year == year && f.Date.Month == month)
                .SumAsync(f => f.Amount);
        }

        public async Task<decimal> CalculateMonthlyTotalExpenses(int year, int month)
        {
            return await _context.Finances
                .Where(f => (f.Type == FinanceType.Wydatek || f.Type == FinanceType.Pensja) && f.Date.Year == year && f.Date.Month == month)
                .SumAsync(f => f.Amount);
        }

        public async Task<decimal> CalculateMonthlyTotalSalaries(int year, int month)
        {
            return await _context.Finances
                .Where(f => f.Type == FinanceType.Pensja && f.Date.Year == year && f.Date.Month == month)
                .SumAsync(f => f.Amount);
        }

        public async Task<decimal> CalculateMonthlyGrossProfit(int year, int month)
        {
            var revenue = await CalculateMonthlyTotalRevenue(year, month);
            var expenses = await CalculateMonthlyTotalExpenses(year, month);
            return revenue - expenses;
        }

        public async Task<decimal> CalculateMonthlyNetProfit(int year, int month)
        {
            var grossProfit = await CalculateMonthlyGrossProfit(year, month);
            decimal taxRate = 0.18m;
            return grossProfit - (grossProfit * taxRate);
        }









        public async Task<decimal> CalculateMonthlyTotalRevenueForUser(string employeeEmail, int year, int month)
        {
            return await _context.Finances
                .Where(f => f.EmployeeEmail == employeeEmail && f.Type == FinanceType.Przychód && f.Date.Year == year && f.Date.Month == month)
                .SumAsync(f => f.Amount);
        }

        public async Task<decimal> CalculateMonthlyTotalExpensesForUser(string employeeEmail, int year, int month)
        {
            return await _context.Finances
                .Where(f => f.EmployeeEmail == employeeEmail && (f.Type == FinanceType.Wydatek || f.Type == FinanceType.Pensja) && f.Date.Year == year && f.Date.Month == month)
                .SumAsync(f => f.Amount);
        }

        public async Task<decimal> CalculateMonthlyTotalSalariesForUser(string employeeEmail, int year, int month)
        {
            return await _context.Finances
                .Where(f => f.EmployeeEmail == employeeEmail && f.Type == FinanceType.Pensja && f.Date.Year == year && f.Date.Month == month)
                .SumAsync(f => f.Amount);
        }

        public async Task<decimal> CalculateMonthlyGrossProfitForUser(string employeeEmail, int year, int month)
        {
            var revenue = await CalculateMonthlyTotalRevenueForUser(employeeEmail, year, month);
            var expenses = await CalculateMonthlyTotalExpensesForUser(employeeEmail, year, month);
            return revenue - expenses;
        }

        public async Task<decimal> CalculateMonthlyNetProfitForUser(string employeeEmail, int year, int month)
        {
            var grossProfit = await CalculateMonthlyGrossProfitForUser(employeeEmail, year, month);
            decimal taxRate = 0.18m;
            return grossProfit - (grossProfit * taxRate);
        }











        public async Task<decimal> CalculateYearTotalRevenue(int year)
        {
            return await _context.Finances
                .Where(f => f.Type == FinanceType.Przychód && f.Date.Year == year)
                .SumAsync(f => f.Amount);
        }

        public async Task<decimal> CalculateYearTotalExpenses(int year)
        {
            return await _context.Finances
                .Where(f => (f.Type == FinanceType.Wydatek || f.Type == FinanceType.Pensja) && f.Date.Year == year)
                .SumAsync(f => f.Amount);
        }

        public async Task<decimal> CalculateYearTotalSalaries(int year)
        {
            return await _context.Finances
                .Where(f => f.Type == FinanceType.Pensja && f.Date.Year == year)
                .SumAsync(f => f.Amount);
        }

        public async Task<decimal> CalculateYearGrossProfit(int year)
        {
            var revenue = await CalculateYearTotalRevenue(year);
            var expenses = await CalculateYearTotalExpenses(year);
            return revenue - expenses;
        }

        public async Task<decimal> CalculateYearNetProfit(int year)
        {
            var grossProfit = await CalculateYearGrossProfit(year);
            decimal taxRate = 0.18m;
            return grossProfit - (grossProfit * taxRate);
        }








        public async Task<decimal> CalculateYearTotalRevenueForUser(string employeeEmail, int year)
        {
            return await _context.Finances
                .Where(f => f.EmployeeEmail == employeeEmail && f.Type == FinanceType.Przychód && f.Date.Year == year)
                .SumAsync(f => f.Amount);
        }

        public async Task<decimal> CalculateYearTotalExpensesForUser(string employeeEmail, int year)
        {
            return await _context.Finances
                .Where(f => f.EmployeeEmail == employeeEmail && (f.Type == FinanceType.Wydatek || f.Type == FinanceType.Pensja) && f.Date.Year == year)
                .SumAsync(f => f.Amount);
        }

        public async Task<decimal> CalculateYearTotalSalariesForUser(string employeeEmail, int year)
        {
            return await _context.Finances
                .Where(f => f.EmployeeEmail == employeeEmail && f.Type == FinanceType.Pensja && f.Date.Year == year)
                .SumAsync(f => f.Amount);
        }

        public async Task<decimal> CalculateYearGrossProfitForUser(string employeeEmail, int year)
        {
            var revenue = await CalculateYearTotalRevenueForUser(employeeEmail, year);
            var expenses = await CalculateYearTotalExpensesForUser(employeeEmail, year);
            return revenue - expenses;
        }

        public async Task<decimal> CalculateYearNetProfitForUser(string employeeEmail, int year)
        {
            var grossProfit = await CalculateYearGrossProfitForUser(employeeEmail, year);
            decimal taxRate = 0.18m;
            return grossProfit - (grossProfit * taxRate);
        }




    }
}
