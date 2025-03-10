using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TransportManagement.Models.Finance;
using TransportManagement.Models.FinanceReport;
using TransportManagement.Models.Orders;
using TransportManagement.Models.User;

namespace TransportManagement.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly TransportManagementDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderService(TransportManagementDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //public async Task<bool> CompleteOrder(int orderId)
        //{
        //    var order = await _context.Orders.FindAsync(orderId);
        //    if (order == null)
        //    {
        //        return false;
        //    }

        //    if (order.Status == OrderStatus.Zakończone)
        //    {
        //        return false;
        //    }

        //    order.Status = OrderStatus.Zakończone;

        //    var finance = new FinanceModel
        //    {
        //        Date = DateTime.UtcNow,
        //        Amount = order.Revenue,
        //        Type = FinanceType.Revenue,
        //        Description = $"Przychód za zlecenie {order.Id}",
        //        DriverEmail = order.DriverEmail
        //    };

        //    var financeReport = new FinanceReportModel
        //    {
        //        DriverEmail = order.DriverEmail,
        //        Year = order.EndDate.Year,
        //        Month = order.EndDate.Month,
        //        TotalRevenue = order.Revenue,
        //        TotalExpenses = 0,
        //        TotalSalary = 0,
        //        TotalProfitFromCompletedOrders = order.Revenue
        //    };

        //    _context.Finances.Add(finance);
        //    _context.FinanceReports.Add(financeReport);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}

        public async Task CreateOrderAsync(DateTime startDate, DateTime endDate, string pickupLocation,
            string deliveryLocation, string driverEmail, string loadType, string assignedBy, decimal revenue)
        {
            string orderNumber = await GenerateOrderNumberAsync();
            var order = new OrderModel
            {
                OrderNumber = orderNumber,
                StartDate = startDate,
                EndDate = endDate,
                PickupLocation = pickupLocation,
                DeliveryLocation = deliveryLocation,
                DriverEmail = driverEmail,
                LoadType = loadType,
                AssignedBy = assignedBy,
                Revenue = revenue
            };
            var priority = GetPriorityForDriver(order);
            order.Priority = priority;
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<OrderModel>> GetAllOrdersAsync()
        {
            //return await _context.Orders.Include(o => o.Driver).ToListAsync();
            return await _context.Orders.ToListAsync() ?? new List<OrderModel>();
        }

        public async Task<OrderModel> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<OrderModel>> GetOrderForDriver(string driverEmail)
        {
            return await _context.Orders.Where(o => o.DriverEmail == driverEmail).ToListAsync();
        }

        public OrderPriority GetPriorityForDriver(OrderModel order)
        {
            var dayLeft = (order.EndDate - DateTime.Now).TotalDays;
            if (dayLeft <= 1)
            {
                return OrderPriority.Wysoki;
            }
            else if (dayLeft <= 3)
            {
                return OrderPriority.Średni;
            }
            else
            {
                return OrderPriority.Niski;
            }
        }

        public async Task<bool> IsDriverOnLeave(string driverEmail, DateTime startDate, DateTime endDate)
        {
            return await _context.LeaveRequests
                        .Where(lr => lr.UserId == driverEmail && lr.Status == Models.LeaveRequests.LeaveStatus.Zaakceptowane).AnyAsync
                        (lr => (startDate >= lr.StartDate && startDate <= lr.EndDate) ||
                        (endDate >= lr.StartDate && endDate <= lr.EndDate) ||
                        (startDate <= lr.StartDate && endDate >= lr.EndDate) ||
                        (startDate <= lr.StartDate && startDate <= lr.EndDate && endDate >= lr.StartDate && endDate <= lr.EndDate));

        }

        public async Task<bool> UpdateOrderAsync(int id, string orderNumber, DateTime startDate, DateTime endDate, string pickupLocation, string deliveryLocation,
             string driverEmail, string loadType, decimal revenue)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (order != null)
            {
                order.OrderNumber = !string.IsNullOrEmpty(orderNumber) ? orderNumber : order.OrderNumber;
                order.StartDate = startDate >= DateTime.Today ? startDate : order.StartDate;
                order.EndDate = endDate >= DateTime.Today ? endDate : order.EndDate;
                order.PickupLocation = !string.IsNullOrEmpty(pickupLocation) ? pickupLocation : order.PickupLocation;
                order.DeliveryLocation = !string.IsNullOrEmpty(deliveryLocation) ? deliveryLocation : order.DeliveryLocation;
                order.Status = order.Status;
                order.LoadType = !string.IsNullOrEmpty(loadType) ? loadType : order.LoadType;
                order.DriverEmail = !string.IsNullOrEmpty(driverEmail) ? driverEmail : order.DriverEmail;
                order.Revenue = revenue >= 0 ? revenue : order.Revenue;

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateOrderStatus(int id, OrderStatus newStatus)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            order.Status = newStatus;

            if (order == null)
            {
                return false;
            }

            order.Status = OrderStatus.Zakończone;
            if (newStatus == OrderStatus.Zakończone)
            {
                var finance = new FinanceModel
                {
                    Date = DateTime.UtcNow,
                    Amount = order.Revenue,
                    Type = FinanceType.Przychód,
                    Description = $"Przychód za zlecenie {order.OrderNumber}",
                    EmployeeEmail = order.DriverEmail
                };
                var financeReport = new FinanceReportModel
                {
                    EmployeeEmail = order.DriverEmail,
                    Year = order.EndDate.Year,
                    Month = order.EndDate.Month,
                    TotalRevenue = order.Revenue,
                    TotalExpenses = 0,
                    TotalSalary = 0,
                    Role = "Driver",
                    TotalProfitFromCompletedOrders = order.Revenue
                };

                _context.Finances.Add(finance);
                _context.FinanceReports.Add(financeReport);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<OrderModel>> GetArchivedOrders()
        {
            return await _context.Orders.Where(o => o.EndDate < DateTime.Now.AddMonths(-1) && o.Status == OrderStatus.Zakończone).ToListAsync();
        }


        public async Task<List<OrderModel>> GetArchivedOrdersForDrivers(string userEmail)
        {
            return await _context.Orders.Where(o => o.DriverEmail == userEmail && o.EndDate < DateTime.Now.AddMonths(-1) && o.Status == OrderStatus.Zakończone).ToListAsync();
        }


        public async Task<string> GenerateOrderNumberAsync()
        {
            string currentMonthYear = DateTime.Now.ToString("MMyy");

            var lastOrder = await _context.Orders
                .Where(o => o.OrderNumber.StartsWith(currentMonthYear))
                .OrderByDescending(o => o.OrderNumber)
                .FirstOrDefaultAsync();

            int newNumber = 1;

            if (lastOrder != null)
            {
                string lastNumberStr = lastOrder.OrderNumber.Substring(4);
                if (int.TryParse(lastNumberStr, out int lastNumber))
                {
                    newNumber = lastNumber + 1;
                }
            }

            return $"{currentMonthYear}{newNumber:D4}";
        }
    }
}