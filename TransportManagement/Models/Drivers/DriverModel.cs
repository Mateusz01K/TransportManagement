using TransportManagement.Models.AssignTruck;

namespace TransportManagement.Models.Drivers
{
    public class DriverModel
    {
        public DriverModel() { }
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int Experience { get; set; }
        public bool IsAssignedTruck { get; set; }
        public ICollection<AssignTruckModel> AssignTruck { get; set; }
    }
}
