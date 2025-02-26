using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TransportManagement.Models.Drivers;
using TransportManagement.Models.User;
using TransportManagement.Services.User;

namespace TransportManagement.Services.Driver
{
    public class DriverService : IDriverService
    {
        private readonly TransportManagementDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IAccountService _accountService;

        public DriverService(TransportManagementDbContext context, UserManager<ApplicationUser> userManager/*, IAccountService accountService*/)
        {
            _context = context;
            _userManager = userManager;
            //_accountService = accountService;
        }

        public async Task<bool> AddDriver(string Name, string LastName, DateTime DateOfBirth, string PhoneNumber, string Email, string Address, int Experience)
        {
            var existingUser = await _userManager.FindByEmailAsync(Email);
            if (existingUser != null)
            {
                if (!await _userManager.IsInRoleAsync(existingUser, "Driver"))
                {
                    await _userManager.AddToRoleAsync(existingUser, "Driver");
                }
            }
            else
            {
                return false;
            }


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
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteDriver(int id)
        {
            var driver = await _context.Drivers.FirstOrDefaultAsync(x => x.Id == id);
            if (driver != null)
            {
                var existingUser = await _userManager.FindByEmailAsync(driver.Email);
                if (existingUser != null)
                {
                    if (await _userManager.IsInRoleAsync(existingUser, "Driver"))
                    {
                        await _userManager.RemoveFromRoleAsync(existingUser, "Driver");
                    }
                }
                _context.Drivers.Remove(driver);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<DriverModel> GetDriver(int id)
        {
            return await _context.Drivers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<DriverModel>> GetDrivers()
        {
            return await _context.Drivers.ToListAsync();
        }

        public async Task<bool> UpdateDriver(int id, string Name, string LastName, DateTime DateOfBirth, string PhoneNumber, string Email, string Address, int Experience)
        {
            var driver = _context.Drivers.FirstOrDefault(x => x.Id == id);
            if (driver != null)
            {
                return false;
            }

            driver.Name = !string.IsNullOrEmpty(Name) ? Name : driver.Name;
            driver.LastName = !string.IsNullOrEmpty(LastName) ? LastName : driver.LastName;
            driver.DateOfBirth = (DateOfBirth != default(DateTime) && DateOfBirth < DateTime.Now) ? DateOfBirth : driver.DateOfBirth;
            driver.PhoneNumber = !string.IsNullOrEmpty(PhoneNumber) ? PhoneNumber : driver.PhoneNumber;
            driver.Email = !string.IsNullOrEmpty(Email) ? Email : driver.Email;
            driver.Address = !string.IsNullOrEmpty(Address) ? Address : driver.Address;
            driver.Experience = Experience >= 0 ? Experience : driver.Experience;
            return true;
        }
    }
}
