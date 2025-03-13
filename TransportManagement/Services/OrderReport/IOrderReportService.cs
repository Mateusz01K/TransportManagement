using TransportManagement.Models.Orders;

namespace TransportManagement.Services.OrderReport
{
    public interface IOrderReportService
    {
        Task<List<OrderModel>> GetCompletedOrder();
        Task<List<OrderModel>> GetCompletedOrderForDriver(string driverEmail);
        Task<byte[]> GeneratePdfReportForDriver(string driverEmail, int year, int month);
    }
}
