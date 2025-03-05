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
    }
}
