using TransportManagement.Models.Drivers;
using TransportManagement.Models.Truck;

namespace TransportManagement.Models.AssignTruck
{
    public class AssignTruckModel
    {
        public AssignTruckModel() { }
        public int Id { get; set; }
        public int TruckId { get; set; }
        public TruckModel Truck { get; set; }
        public int DriverId { get; set; }
        public DriverModel Driver { get; set; }
        public DateTime AssignmentDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool IsReturned { get; set; }
    }
}
