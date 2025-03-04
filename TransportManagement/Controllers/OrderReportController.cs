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
    }
}
