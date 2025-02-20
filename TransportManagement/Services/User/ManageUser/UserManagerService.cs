using Microsoft.AspNetCore.Identity;
using TransportManagement.Models.User;

namespace TransportManagement.Services.User.ManageUser
{
    public class UserManagerService : IUserManagerService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagerService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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
    }
}
