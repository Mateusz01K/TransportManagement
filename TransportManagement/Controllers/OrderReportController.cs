using Microsoft.AspNetCore.Mvc;
using TransportManagement.Models.Orders;
using TransportManagement.Services.OrderReport;

namespace TransportManagement.Controllers
{
    public class OrderReportController : Controller
    {
        private readonly IOrderReportService _orderReportService;

        public OrderReportController(IOrderReportService orderReportService)
        {
            _orderReportService = orderReportService;
        }

        [HttpGet]
        public async Task<IActionResult> Reports()
        {
            var completedOrders = await _orderReportService.GetCompletedOrder();
            var orderViewModel = new OrderViewModel
            {
                Orders = completedOrders
            };
            return View(orderViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GeneratePdfReport(string driverEmail, int year, int month)
        {
            try
            {
                var pdfBytes = await _orderReportService.GeneratePdfReportForDriver(driverEmail, year, month);

                return File(pdfBytes, "application/pdf", $"Raport_zleceń_{driverEmail}_{year}_{month}.pdf");
            }
            catch (Exception ex)
            {
                TempData["message"] = $"Wystąpił błąd: {ex.Message}";
                return RedirectToAction("Reports");
            }
        }
    }
}