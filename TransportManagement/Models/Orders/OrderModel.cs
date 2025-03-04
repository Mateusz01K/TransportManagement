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
        public OrderPriority Priority { get; set; }
        public OrderStatus Status { get; set; }
        public string LoadType { get; set; }
        public string DriverEmail { get; set; }
        public decimal Revenue { get; set; }

        public string AssignedBy { get; set; }
        public ApplicationUser Dispatcher { get; set; }
    }

    public enum OrderStatus
    {
        Oczekujące,
        Trwa,
        Dostarczone,
        Zakończone,
        Anulowane
        //Pending,
        //InProgress,
        //Completed,
        //Canceled
    }

    public enum OrderPriority
    {
        Niski,
        Średni,
        Wysoki
    }
}
