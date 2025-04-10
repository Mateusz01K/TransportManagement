using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportManagement.Models.Orders;
using TransportManagement.Models.User;
using TransportManagement.Services.Order;

namespace TransportManagement.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly TransportManagementDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderController(ILogger<OrderController> logger, IOrderService orderService, TransportManagementDbContext context, UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin, Dispatcher")]
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

        [Authorize(Roles = "Admin, Dispatcher")]
        [HttpPost]
        public async Task<IActionResult> Create(DateTime startDate, DateTime endDate, string pickupLocation,
            string deliveryLocation, string driverEmail, string loadType, decimal revenue)
        {
            if (startDate < DateTime.Today || endDate < DateTime.Today || string.IsNullOrEmpty(pickupLocation) ||
                string.IsNullOrEmpty(deliveryLocation) || string.IsNullOrEmpty(driverEmail) || string.IsNullOrEmpty(loadType) || revenue < 0)
            {
                TempData["message"] = "Popraw Dane.";
                return RedirectToAction("Index");
            }

            var user = await _userManager.FindByEmailAsync(driverEmail);
            if (user == null || !await _userManager.IsInRoleAsync(user, "Driver"))
            {
                TempData["message"] = "Podany użytkownik nie jest kierowcą.";
                return RedirectToAction("Index");
            }

            bool isOnLeave = await _orderService.IsDriverOnLeave(driverEmail, startDate, endDate);
            if (isOnLeave)
            {
                TempData["message"] = "Kierowca w tym czasie jest na urlopie.";
                return RedirectToAction("Index");
            }
            string assignedBy = User.Identity?.Name;
            await _orderService.CreateOrderAsync(startDate, endDate, pickupLocation, deliveryLocation, driverEmail, loadType, assignedBy, revenue);
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Admin, Dispatcher")]
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            if (orders == null || !orders.Any())
            {
                TempData["message"] = "Nie można edytować - Zlecenie nie istnieje.";
                return RedirectToAction("Index");
            }

            var orderView = new OrderViewModel
            {
                Orders = orders
            };
            return View(orderView);
        }


        [Authorize(Roles = "Admin, Dispatcher")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, string orderNumber, DateTime startDate, DateTime endDate, string pickupLocation,
            string deliveryLocation, string driverEmail, string loadType, decimal revenue)
        {
            if (id != 0)
            {
                await _orderService.UpdateOrderAsync(id, orderNumber, startDate, endDate, pickupLocation, deliveryLocation, driverEmail, loadType, revenue);
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Popraw Dane.";
            return View("Index");
        }

        [Authorize(Roles = "Admin, Dispatcher, Driver")]
        public async Task<IActionResult> Index()
        {
            var userEmail = User.Identity?.Name;
            var isDriver = User.IsInRole("Driver");
            List<OrderModel> orders = await _orderService.GetAllOrdersAsync() ?? new List<OrderModel>();
            if (isDriver)
            {
                if (string.IsNullOrEmpty(userEmail))
                {
                    TempData["message"] = "Nie można pobrać e-maila użytkownika.";
                    return RedirectToAction("Index");
                }
                orders = await _orderService.GetOrderForDriver(userEmail) ?? new List<OrderModel>();
            }
            else
            {
                orders = await _orderService.GetAllOrdersAsync() ?? new List<OrderModel>();
            }
            var orderViewModel = new OrderViewModel()
            {
                Orders = orders
            };
            if (isDriver)
            {
                return View("DriverIndex", orderViewModel);
            }
            else
            {
                return View("Index", orderViewModel);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Dispatcher")]
        public async Task<IActionResult> UpdateStatus(int orderId, OrderStatus newStatus)
        {
            var success = await _orderService.UpdateOrderStatus(orderId, newStatus);
            if (!success)
            {
                TempData["message"] = "Nie udało się zmienić statusu";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> MarkDelivered(int orderId)
        {
            var success = await _orderService.UpdateOrderStatus(orderId, OrderStatus.Dostarczone);
            if (!success)
            {
                TempData["message"] = "Nie udało się zmienić statusu";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetDriverEmails(string term)
        {
            var emails = await _context.Drivers.Where(d => d.Email.Contains(term)).Select(d => d.Email).ToListAsync();
            return Json(emails);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Dispatcher")]
        public async Task<IActionResult> ArchivedOrders()
        {
            var archiverOrders = await _orderService.GetArchivedOrders();
            var model = new OrderViewModel
            {
                Orders = archiverOrders
            };
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> ArchivedOrdersForDrivers()
        {
            string userEmail = User.Identity.Name;
            var archiverOrders = await _orderService.GetArchivedOrdersForDrivers(userEmail);
            var model = new OrderViewModel
            {
                Orders = archiverOrders
            };
            return View(model);
        }
    }
}
