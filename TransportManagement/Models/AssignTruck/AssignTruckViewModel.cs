using TransportManagement.Models.Drivers;
using TransportManagement.Models.Truck;

namespace TransportManagement.Models.AssignTruck
{
    public class AssignTruckViewModel
    {
        public AssignTruckViewModel() { }
        public List<AssignTruckModel> AssignTrucks { get; set; }
        public List<TruckModel> Trucks { get; set; }
        public List<DriverModel> Drivers { get; set; }
    }
}
