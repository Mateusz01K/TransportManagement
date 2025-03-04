using Microsoft.EntityFrameworkCore;
using TransportManagement.Models.Orders;

namespace TransportManagement.Services.OrderReport
{
    public class OrderReportService : IOrderReportService
    {
        private readonly TransportManagementDbContext _context;

        public OrderReportService(TransportManagementDbContext context)
        {
            _context = context;
        }
        public async Task<List<OrderModel>> GetCompletedOrder()
        {
            return await _context.Orders.Where(o => o.Status == OrderStatus.Zakończone).ToListAsync();
        }
    }
}
