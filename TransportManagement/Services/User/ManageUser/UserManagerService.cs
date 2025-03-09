using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TransportManagement.Models.User;

namespace TransportManagement.Services.User.ManageUser
{
    public class UserManagerService : IUserManagerService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TransportManagementDbContext _context;

        public UserManagerService(UserManager<ApplicationUser> userManager, TransportManagementDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<List<UserDto>> GetUserAsync()
        {
            var users = _userManager.Users.ToList();
            var userList = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new UserDto
                {
                    Name = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Role = roles.Any() ? string.Join(", ", roles) : "Brak"
                });
            }
            return userList;
        }


        public async Task<bool> UpdateUserAsync(string Email, string FirstName, string LastName, DateTime DateOfBirth, string PhoneNumber, string Address, int Experience, decimal Salary)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return false;
            }
            user.FirstName = !string.IsNullOrEmpty(FirstName) ? FirstName : user.FirstName;
            user.LastName = !string.IsNullOrEmpty(LastName) ? LastName : user.LastName;
            user.DateOfBirth = (DateOfBirth != default(DateTime) && DateOfBirth < DateTime.Now) ? DateOfBirth : user.DateOfBirth;
            user.PhoneNumber = !string.IsNullOrEmpty(PhoneNumber) ? PhoneNumber : user.PhoneNumber;
            user.Email = !string.IsNullOrEmpty(Email) ? Email : user.Email;
            user.Address = !string.IsNullOrEmpty(Address) ? Address : user.Address;
            user.Experience = Experience >= 0 ? Experience : user.Experience;
            user.Salary = Salary >= 0 ? Salary : user.Salary;

            var result = await _userManager.UpdateAsync(user);
            var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.Email == Email);
            if (driver != null)
            {
                driver.Name = user.FirstName;
                driver.LastName = user.LastName;
                driver.DateOfBirth = user.DateOfBirth;
                driver.PhoneNumber = user.PhoneNumber;
                driver.Address = user.Address;
                driver.Experience = user.Experience;
                driver.Salary = user.Salary;

                await _context.SaveChangesAsync();
            }
            return result.Succeeded;
        }


        public async Task<bool> DeleteUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            if (await _userManager.IsInRoleAsync(user, "Driver"))
            {
                var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.Email == user.Email);
                if (driver != null)
                {
                    _context.Drivers.Remove(driver);
                    await _context.SaveChangesAsync();
                }
            }
            var result = await _userManager.DeleteAsync(user);
            return true;
        }
    }
}
