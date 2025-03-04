using TransportManagement.Models.Drivers;

namespace TransportManagement.Services.Driver
{
    public interface IDriverService
    {
        Task<List<DriverModel>> GetDrivers();
        Task<DriverModel> GetDriver(int id);
        Task<bool> AddDriver(string Name, string LastName, DateTime DateOfBirth, string PhoneNumber, string Email, string Address, int Experience, decimal Salary);
        //public void AddDriver(string Name, string LastName, DateTime DateOfBirth, string PhoneNumber, string Email, string Address, int Experience);
        Task<bool> UpdateDriver(int id, string Name, string LastName, DateTime DateOfBirth, string PhoneNumber, string Email, string Address, int Experience, decimal Salary);
        Task<bool> DeleteDriver(int id);


        /*
        public List<DriverModel> GetDrivers();
        public DriverModel GetDriver(int id);
        public void AddDriver(string Name, string LastName, DateTime DateOfBirth, string PhoneNumber, string Email, string Address, int Experience);
        public void UpdateDriver(int id, string Name, string LastName, DateTime DateOfBirth, string PhoneNumber, string Email, string Address, int Experience);
        public void DeleteDriver(int id);
        */
    }
}
