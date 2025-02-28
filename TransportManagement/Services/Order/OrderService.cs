using Microsoft.EntityFrameworkCore;
using TransportManagement.Models.Orders;

namespace TransportManagement.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly TransportManagementDbContext _context;

        public OrderService(TransportManagementDbContext context)
        {
            _context = context;
        }
        public async Task CreateOrderAsync(OrderModel order)
        {
            order.Status = OrderStatus.Pending;
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if(order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<string>> GetAllDriverEmails()
        {
            return await _context.Users.Where(u=>u.Role=="Driver").Select(u=>u.Email).ToListAsync();
        }

        public async Task<List<OrderModel>> GetAllOrdersAsync()
        {
            return await _context.Orders.Include(o=>o.Driver).ToListAsync();
        }

        public async Task<OrderModel> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.Include(o => o.Driver).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<OrderModel>> GetOrderForDriver(string driverEmail)
        {
            return await _context.Orders.Where(o => o.DriverEmail == driverEmail).ToListAsync();
        }

        public async Task UpdateOrderAsync(OrderModel order)
        {
            var existingOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
            if(existingOrder != null)
            {
                existingOrder.OrderNumber = order.OrderNumber;
                existingOrder.StartDate = order.StartDate;
                existingOrder.EndDate = order.EndDate;
                existingOrder.PickupLocation = order.PickupLocation;
                existingOrder.DeliveryLocation = order.DeliveryLocation;
                existingOrder.Status = OrderStatus.Pending;
                existingOrder.DriverId = order.DriverId;
                existingOrder.SequenceNumber = order.SequenceNumber;

                await _context.SaveChangesAsync();
            }
        }
    }
}