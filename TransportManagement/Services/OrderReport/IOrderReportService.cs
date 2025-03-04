using TransportManagement.Models.Orders;

namespace TransportManagement.Services.OrderReport
{
    public interface IOrderReportService
    {
        Task<List<OrderModel>> GetCompletedOrder();
    }
}
