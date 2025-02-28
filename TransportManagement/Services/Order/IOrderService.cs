using TransportManagement.Models.Orders;

namespace TransportManagement.Services.Order
{
    public interface IOrderService
    {
        Task<List<OrderModel>> GetAllOrdersAsync();
        Task<OrderModel> GetOrderByIdAsync(int id);
        //Task CreateOrderAsync(int orderNumber, DateTime startDate, DateTime endDate, string pickupLocation, string deliveryLocation, int driverId, string status, int sequenceNumber, string assignedBy);
        Task CreateOrderAsync(OrderModel order);
        Task UpdateOrderAsync(OrderModel order);
        //Task AssignOrderToDriverAsync(int id, string driverId);
        Task DeleteOrderAsync(int id);
        Task<List<string>> GetAllDriverEmails();
        Task<List<OrderModel>> GetOrderForDriver(string driverEmail);
    }
}