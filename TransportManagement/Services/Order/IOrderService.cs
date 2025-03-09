using TransportManagement.Models.Orders;

namespace TransportManagement.Services.Order
{
    public interface IOrderService
    {
        Task<List<OrderModel>> GetAllOrdersAsync();
        //Task<OrderModel> GetOrderByIdAsync(int id);
        Task CreateOrderAsync(string orderNumber, DateTime startDate, DateTime endDate, string pickupLocation, string deliveryLocation,
             string driverEmail, string loadType, string assignedBy, decimal revenue);
        Task<bool> UpdateOrderAsync(int id, string orderNumber, DateTime startDate, DateTime endDate, string pickupLocation, string deliveryLocation,
             string driverEmail, string loadType, decimal revenue);
        Task DeleteOrderAsync(int id);
        Task<List<OrderModel>> GetOrderForDriver(string driverEmail);
        Task<bool> UpdateOrderStatus(int id, OrderStatus newStatus);

        OrderPriority GetPriorityForDriver(OrderModel order);
        Task<bool> IsDriverOnLeave(string driverEmail, DateTime startDate, DateTime endDate);

        Task<List<OrderModel>> GetArchivedOrders();
        Task<List<OrderModel>> GetArchivedOrdersForDrivers(string userEmail);
        //Task<bool> CompleteOrder(int orderId);

        //Task AssignOrderToDriverAsync(int id, string driverEmail);
        //Task CreateOrderAsync(OrderModel order);
    }
}