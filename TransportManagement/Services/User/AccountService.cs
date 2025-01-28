using Microsoft.AspNetCore.Identity;
using TransportManagement.Models.User;
using TransportManagement.Services.User.EmailSender;

namespace TransportManagement.Services.User
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
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
            if(user == null)
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

            if(result.Succeeded)
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
            return result.Succeeded;
        }


        public async Task<List<UserDto>> GetUserAsync()
        {
            var users = _userManager.Users.ToList();
            var userList = new List<UserDto>();

            foreach(var user in users)
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

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
