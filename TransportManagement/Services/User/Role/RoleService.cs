using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TransportManagement.Models.User;
using TransportManagement.Services.Driver;

namespace TransportManagement.Services.User.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TransportManagementDbContext _context;
        private readonly IDriverService _driverService;

        public RoleService(UserManager<ApplicationUser> userManager, TransportManagementDbContext context, IDriverService driverService)
        {
            _userManager = userManager;
            _context = context;
            _driverService = driverService;
        }

        public async Task<bool> AssignRoleAsync(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded && roleName == "Driver")
            {
                var existingUser = await _context.Drivers.FirstOrDefaultAsync(d => d.Email == user.Email);
                if (existingUser == null)
                {
                    bool driverAdded = await _driverService.AddDriver(
                        user.FirstName,
                        user.LastName,
                        user.DateOfBirth,
                        user.PhoneNumber,
                        user.Email,
                        user.Address,
                        user.Experience,
                        user.Salary
                        );
                }
            }
            return result.Succeeded;
        }

        public async Task<bool> UnAssignRoleAsync(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded && roleName == "Driver")
            {
                var existingUser = await _context.Drivers.FirstOrDefaultAsync(d => d.Email == user.Email);
                if (existingUser != null)
                {
                    bool driverAdded = await _driverService.DeleteDriver(existingUser.Id);
                }
            }

            return result.Succeeded;
        }
    }
}
