using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Xml.Linq;
using TransportManagement.Models.Drivers;
using TransportManagement.Models.User;
using TransportManagement.Services.Driver;
using TransportManagement.Services.User.EmailSender;

namespace TransportManagement.Services.User
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IDriverService _driverService;
        private readonly TransportManagementDbContext _context;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, IDriverService driverService
                            , TransportManagementDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _driverService = driverService;
            _context = context;
        }

        public async Task SendPasswordResetEmailAsync(string email, string resetUrlBase)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetUrl = $"{resetUrlBase}?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            await _emailSender.SendEmailAsync(email, "Reset hasła",
                $"Aby zresetować hasło, kliknij w poniższy link: <a href='{resetUrl}'>Resetuj hasło.</a>");
        }

        public async Task<bool> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;

        }

        public async Task<ApplicationUser> LoginUserAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return null;
            }

            var result = await _signInManager.PasswordSignInAsync
                (
                    model.Email,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false
                );

            if (result.Succeeded)
            {
                return user;
            }

            if (result.IsLockedOut)
            {
                throw new Exception("Konto uzytkownika jest zablokowane.");
            }
            return null;
        }

        public async Task<bool> RegisterUserAsync(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Experience = model.Experience,
                DateOfBirth = model.DateOfBirth,
                Role = ""
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            return result.Succeeded;
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
                        user.Experience
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

        public async Task<bool> UpdateUserAsync(string Email, string FirstName, string LastName, DateTime DateOfBirth, string PhoneNumber, string Address, int Experience)
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

                await _context.SaveChangesAsync();
            }
            return result.Succeeded;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
