using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportManagement.Models.Finance;
using TransportManagement.Services.Finance;

namespace TransportManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FinanceController : Controller
    {
        private readonly IFinanceService _financeService;

        public FinanceController(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        public async Task<IActionResult> Finance()
        {
            //var finances = await _financeService.GetAllFinance();
            var revenue = await _financeService.CalculateTotalRevenue();
            var expenses = await _financeService.CalculateTotalExpenses();
            var salary = await _financeService.CalculateTotalSalaries();
            var grossProfit = await _financeService.CalculateGrossProfit();
            var netProfit = await _financeService.CalculateNetProfit();
            var viewModel = new FinanceViewModel
            {
                //Finances = finances,
                TotalRevenue = revenue,
                TotalExpenses = expenses,
                TotalSalaries = salary,
                GrossProfit = grossProfit,
                NetProfit = netProfit
            };
            return View(viewModel);
        }

        public async Task<IActionResult> AddSalaries(int year, int month, int day)
        {
            await _financeService.AddSalariesToFinance(year, month, day);
            return RedirectToAction("Finance");
        }

        public async Task<IActionResult> AddExpense(decimal amount, string description, int year, int month, int day)
        {
            await _financeService.AddExpensesToFinance(amount, description, year, month, day);
            return RedirectToAction("Finance");
        }



        public async Task<IActionResult> MonthlyFinance(int year, int month)
        {
            var revenue = await _financeService.CalculateMonthlyTotalRevenue(year, month);
            var expenses = await _financeService.CalculateMonthlyTotalExpenses(year, month);
            var salary = await _financeService.CalculateMonthlyTotalSalaries(year, month);
            var grossProfit = await _financeService.CalculateMonthlyGrossProfit(year, month);
            var netProfit = await _financeService.CalculateMonthlyNetProfit(year, month);
            var finaces = await _financeService.GetAllFinance();

            var viewModel = new FinanceViewModel
            {
                Finances = finaces,
                Year = year,
                Month = month,
                TotalRevenue = revenue,
                TotalExpenses = expenses,
                TotalSalaries = salary,
                GrossProfit = grossProfit,
                NetProfit = netProfit
            };

            return View(viewModel);
        }



        public async Task<IActionResult> MonthlyFinanceForUser(string employeeEmail, int year, int month)
        {
            var revenue = await _financeService.CalculateMonthlyTotalRevenueForUser(employeeEmail, year, month);
            var expenses = await _financeService.CalculateMonthlyTotalExpensesForUser(employeeEmail, year, month);
            var salary = await _financeService.CalculateMonthlyTotalSalariesForUser(employeeEmail, year, month);
            var grossProfit = await _financeService.CalculateMonthlyGrossProfitForUser(employeeEmail, year, month);
            var netProfit = await _financeService.CalculateMonthlyNetProfitForUser(employeeEmail, year, month);
            var finaces = await _financeService.GetAllFinance();

            var viewModel = new FinanceViewModel
            {
                Finances = finaces,
                EmployeeEmail = employeeEmail,
                Year = year,
                Month = month,
                TotalRevenue = revenue,
                TotalExpenses = expenses,
                TotalSalaries = salary,
                GrossProfit = grossProfit,
                NetProfit = netProfit
            };

            return View(viewModel);
        }




        public async Task<IActionResult> YearFinance(int year)
        {
            var revenue = await _financeService.CalculateYearTotalRevenue(year);
            var expenses = await _financeService.CalculateYearTotalExpenses(year);
            var salary = await _financeService.CalculateYearTotalSalaries(year);
            var grossProfit = await _financeService.CalculateYearGrossProfit(year);
            var netProfit = await _financeService.CalculateYearNetProfit(year);
            var finaces = await _financeService.GetAllFinance();

            var viewModel = new FinanceViewModel
            {
                Finances = finaces,
                Year = year,
                TotalRevenue = revenue,
                TotalExpenses = expenses,
                TotalSalaries = salary,
                GrossProfit = grossProfit,
                NetProfit = netProfit
            };

            return View(viewModel);
        }



        public async Task<IActionResult> YearFinanceForUser(string employeeEmail, int year)
        {
            var revenue = await _financeService.CalculateYearTotalRevenueForUser(employeeEmail, year);
            var expenses = await _financeService.CalculateYearTotalExpensesForUser(employeeEmail, year);
            var salary = await _financeService.CalculateYearTotalSalariesForUser(employeeEmail, year);
            var grossProfit = await _financeService.CalculateYearGrossProfitForUser(employeeEmail, year);
            var netProfit = await _financeService.CalculateYearNetProfitForUser(employeeEmail, year);
            var finaces = await _financeService.GetAllFinance();

            var viewModel = new FinanceViewModel
            {
                Finances = finaces,
                EmployeeEmail = employeeEmail,
                Year = year,
                TotalRevenue = revenue,
                TotalExpenses = expenses,
                TotalSalaries = salary,
                GrossProfit = grossProfit,
                NetProfit = netProfit
            };

            return View(viewModel);
        }
    }
}
