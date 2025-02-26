using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportManagement.Models.Drivers;
using TransportManagement.Models.Orders;
using TransportManagement.Services.Order;

namespace TransportManagement.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = "Dispatcher")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            var orderViewModel = new OrderViewModel
            {
                Orders = orders
            };
            return View(orderViewModel);
        }

        [Authorize(Roles = "Dispatcher")]
        [HttpPost]
        public async Task<IActionResult> Create(OrderModel order)
        {
            if(!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Popraw Dane.";
                return View(order);
            }
            await _orderService.CreateOrderAsync(order);
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Dispatcher")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                TempData["ErrorMessage"] = "Zlecenia nie ma w bazie danych.";
                return View(order);
            }
           return View(order);
        }


        [Authorize(Roles = "Dispatcher")]
        [HttpPost]
        public async Task<IActionResult> Edit(OrderModel order)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Popraw Dane.";
                return View(order);
            }

            await _orderService.UpdateOrderAsync(order);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Dispatcher, Driver")]
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }
    }
}
