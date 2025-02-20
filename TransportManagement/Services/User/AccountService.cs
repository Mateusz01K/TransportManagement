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

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
