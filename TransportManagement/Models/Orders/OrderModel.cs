using TransportManagement.Models.Drivers;

namespace TransportManagement.Models.Orders
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PickupLocation { get; set; }
        public string DeliveryLocation { get; set; }
        public OrderStatus Status { get; set; }


        public int DriverId { get; set; }
        public DriverModel Driver { get; set; }
    }

    public enum OrderStatus
    {
        Pending,
        InProgress,
        Completed,
        Canceled
    }
}
