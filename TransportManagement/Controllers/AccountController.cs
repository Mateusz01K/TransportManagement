using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TransportManagement.Models.User;
using TransportManagement.Services.User;
using TransportManagement.Services.User.EmailSender;
using TransportManagement.Services.User.ManageUser;

namespace TransportManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public AccountController(IAccountService accountService, IUserService userService, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _accountService = accountService;
            _userService = userService;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public IActionResult RegisterView()
        {
            return View();
        }

        public async Task<IActionResult> RegisterUser(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingUser = await _userService.GetUserByEmail(model.Email);
            if(existingUser != null)
            {
                TempData["message"] = "Konto o tym adresie e-mail już istnieje.";
                return View(model);
            }

            var success = await _accountService.RegisterUserAsync(model);

            if (success)
            {
                TempData["message"] = "Rejestracja zakończona sukcesem.";
                return RedirectToAction("LoginUser");
            }

            ModelState.AddModelError("", "Rejestracja nie powiodla sie.");
            return View(model);
        }

        public IActionResult LoginView()
        {
            return View();
        }

        public async Task<IActionResult> LoginUser(LoginViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            if(string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password)){
                ModelState.AddModelError(string.Empty, "E-mail i hasło są wymagane.");
                return View(model);
            }


            var user = await _accountService.LoginUserAsync(model);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Nieprawidłowe hasło lub e-mail.");
                return View(model);
            }

            if(user.HasChangedPassword)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("ChangePassword", "Account");
            }
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if(model.NewPassword != model.ConfirmPassword)
            {
                TempData["message"] = "Hasła musza być takie same..";
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("LoginUser");
            }

            var result = await _accountService.ChangePassword(User.Identity.Name, model.CurrentPassword, model.NewPassword, model.ConfirmPassword);
            if(result)
            {
                TempData["message"] = "Hasło zostało zmienione.";
                return RedirectToAction("Index", "Home");
            }
            TempData["message"] = "Zmiana hasła nie powiodła się.";
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction("LoginUser");
        }

        public async Task<IActionResult> TestEmail()
        {
            var email = "transportaptest1@onet.pl";
            var subject = "Testowy e-mail";
            var message = "Test";
            await _emailSender.SendEmailAsync(email, subject, message);
            return Content("E-mail wysłany");
        }
    }
}