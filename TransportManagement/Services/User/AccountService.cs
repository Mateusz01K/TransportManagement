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
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return false;
            }

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
                Role = "",
                HasChangedPassword = false
            };

            var tempPassword = GenerateSecurePassword();/*"Tmp" + Guid.NewGuid().ToString("N").Substring(0, 4) + "!";*/
            if (tempPassword.Length < 6)
            {
                tempPassword = tempPassword.PadRight(6, '1');
            }
            var result = await _userManager.CreateAsync(user, tempPassword);
            if (!result.Succeeded)
            {
                return false;
            }

            await _userManager.SetLockoutEnabledAsync(user, false);
            await _userManager.UpdateSecurityStampAsync(user);
            string message = $"Twoje tymczasowe hasło: {tempPassword} . Zaloguj się i zmień hasło.";
            await _emailSender.SendEmailAsync(user.Email, "Twoje konto w systemie zostało utworzone.", message);
            return true;

            //var result = await _userManager.CreateAsync(user, model.Password);
            //return result.Succeeded;
        }


        //public async Task<bool> RegisterUserAsync(RegisterViewModel model)
        //{

        //    var user = new ApplicationUser
        //    {
        //        UserName = model.Email,
        //        Email = model.Email,
        //        FirstName = model.FirstName,
        //        LastName = model.LastName,
        //        PhoneNumber = model.PhoneNumber,
        //        Address = model.Address,
        //        Experience = model.Experience,
        //        DateOfBirth = model.DateOfBirth,
        //        Role = ""
        //    };

        //    var result = await _userManager.CreateAsync(user, model.Password);
        //    return result.Succeeded;
        //}

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> ChangePassword(string userId, string currentPassword, string newPassword, string confirmPassword)
        {
            var user = await _userManager.FindByEmailAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
            {
                return false;

            }
            //await _userManager.UpdateSecurityStampAsync(user);
            user.HasChangedPassword = true;
            await _userManager.UpdateAsync(user);
            return true;
        }

        private string GenerateSecurePassword()
        {
            var passwordOptions = new PasswordOptions
            {
                RequiredLength = 12,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
                RequireNonAlphanumeric = true
            };

            string[] randomChars = {
        "ABCDEFGHJKLMNOPQRSTUVWXYZ",
        "abcdefghijkmnopqrstuvwxyz",
        "0123456789",
        "!@$?_-."
    };

            Random rand = new Random();
            List<char> password = new List<char>();

            password.Add(randomChars[0][rand.Next(randomChars[0].Length)]);
            password.Add(randomChars[1][rand.Next(randomChars[1].Length)]);
            password.Add(randomChars[2][rand.Next(randomChars[2].Length)]);
            password.Add(randomChars[3][rand.Next(randomChars[3].Length)]);

            for (int i = 4; i < passwordOptions.RequiredLength; i++)
            {
                string randomSet = randomChars[rand.Next(randomChars.Length)];
                password.Add(randomSet[rand.Next(randomSet.Length)]);
            }

            return new string(password.OrderBy(x => rand.Next()).ToArray());
        }
    }
}
