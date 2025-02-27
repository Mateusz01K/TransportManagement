using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TransportManagement.Models.User;

namespace TransportManagement.Services.User
{
    public class UserService : IUserService
    {
        private readonly TransportManagementDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(TransportManagementDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<ApplicationUser> GetCurrentUser(ClaimsPrincipal user)
        {
            string email = user.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }
            return await GetUserByEmail(email);
        }

        public async Task<Dictionary<string, string>> GetAllUsersEmails()
        {
            return await _userManager.Users.ToDictionaryAsync(u => u.Id, u => u.Email);
        }
    }
}
