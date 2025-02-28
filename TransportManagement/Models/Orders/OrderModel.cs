using TransportManagement.Models.Drivers;
using TransportManagement.Models.User;

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
        public string LoadType { get; set; }
        public string DriverEmail { get; set; }


        public int DriverId { get; set; }
        public DriverModel Driver { get; set; }

        public string AssignedBy { get; set; }
        public ApplicationUser Dispatcher { get; set; }


        public int? SequenceNumber { get; set; }
    }

    public enum OrderStatus
    {
        Pending,
        InProgress,
        Completed,
        Canceled
    }
}
