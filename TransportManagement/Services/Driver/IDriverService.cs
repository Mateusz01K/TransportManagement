using TransportManagement.Models.Drivers;

namespace TransportManagement.Services.Driver
{
    public interface IDriverService
    {
        List<DriverModel> GetDrivers();
        public DriverModel GetDriver(int id);
        public void AddDriver(string Name, string LastName, DateTime DateOfBirth, string PhoneNumber, string Email, string Address, int Experience);
        public void UpdateDriver(int id, string Name, string LastName, DateTime DateOfBirth, string PhoneNumber, string Email, string Address, int Experience);
        public void DeleteDriver(int id);


        /*
        public List<DriverModel> GetDrivers();
        public DriverModel GetDriver(int id);
        public void AddDriver(string Name, string LastName, DateTime DateOfBirth, string PhoneNumber, string Email, string Address, int Experience);
        public void UpdateDriver(int id, string Name, string LastName, DateTime DateOfBirth, string PhoneNumber, string Email, string Address, int Experience);
        public void DeleteDriver(int id);
        */
    }
}
