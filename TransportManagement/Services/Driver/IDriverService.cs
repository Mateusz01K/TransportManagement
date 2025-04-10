using TransportManagement.Models.Drivers;

namespace TransportManagement.Services.Driver
{
    public interface IDriverService
    {
        Task<List<DriverModel>> GetDrivers();
        Task<DriverModel> GetDriver(int id);
        Task<bool> AddDriver(string Name, string LastName, DateTime DateOfBirth, string PhoneNumber, string Email, string Address, int Experience, decimal Salary);
        Task<bool> UpdateDriver(int id, string Name, string LastName, DateTime DateOfBirth, string PhoneNumber, string Email, string Address, int Experience, decimal Salary);
        Task<bool> DeleteDriver(int id);
    }
}
