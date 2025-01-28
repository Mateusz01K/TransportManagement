using Microsoft.EntityFrameworkCore;
using TransportManagement.Models.Drivers;

namespace TransportManagement.Services.Driver
{
    public class DriverService : IDriverService
    {
        private readonly TransportManagementDbContext _context;

        public DriverService(TransportManagementDbContext context)
        {
            _context = context;
        }

        public void AddDriver(string Name, string LastName, DateTime DateOfBirth, string PhoneNumber, string Email, string Address, int Experience)
        {
            var driver = new DriverModel
            {
                Name = Name,
                LastName = LastName,
                DateOfBirth = DateOfBirth,
                PhoneNumber = PhoneNumber,
                Email = Email,
                Address = Address,
                Experience = Experience
            };
            _context.Drivers.Add(driver);
            _context.SaveChanges();
        }

        public void DeleteDriver(int id)
        {
            var driver = _context.Drivers.FirstOrDefault(x => x.Id == id);
            if (driver != null)
            {
                _context.Drivers.Remove(driver);
                _context.SaveChanges();
            }
        }

        public DriverModel GetDriver(int id)
        {
            var driver = _context.Drivers.FirstOrDefault(x => x.Id == id);
            return driver ?? new DriverModel();
        }

        public List<DriverModel> GetDrivers()
        {
            return _context.Drivers.ToList();
        }

        public void UpdateDriver(int id, string Name, string LastName, DateTime DateOfBirth, string PhoneNumber, string Email, string Address, int Experience)
        {
            var driver = _context.Drivers.FirstOrDefault(x => x.Id == id);
            if (driver != null)
            {
                driver.Name = !string.IsNullOrEmpty(Name) ? Name : driver.Name;
                driver.LastName = !string.IsNullOrEmpty(LastName) ? LastName : driver.LastName;
                driver.DateOfBirth = (DateOfBirth != default(DateTime) && DateOfBirth < DateTime.Now) ? DateOfBirth : driver.DateOfBirth;
                driver.PhoneNumber = !string.IsNullOrEmpty(PhoneNumber) ? PhoneNumber : driver.PhoneNumber;
                driver.Email = !string.IsNullOrEmpty(Email) ? Email : driver.Email;
                driver.Address = !string.IsNullOrEmpty(Address) ? Address : driver.Address;
                driver.Experience = Experience >= 0 ? Experience : driver.Experience;
                _context.SaveChanges();
            }
        }
    }
}
