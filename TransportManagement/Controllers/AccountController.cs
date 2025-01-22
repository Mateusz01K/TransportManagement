using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TransportManagement.Models.User;
using TransportManagement.Models.User.ResetPassword;
using TransportManagement.Services.User;

namespace TransportManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
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
            TempData["message"] = "Zostales zalogowany.";
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            TempData["message"] = "wylogowano pomyslnie.";
            return RedirectToAction("LoginUser");
        }


        public IActionResult ForgotPasswordView()
        {
            return View();
        }

        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ForgotPassword", model);
            }

            var resetUrlBase = Url.Action("ResetPassword", "Account", null, Request.Scheme);
            await _accountService.SendPasswordResetEmailAsync(model.Email, resetUrlBase);

            TempData["message"] = "Otrzymasz wiadomośc e-mail.";
            return RedirectToAction("ForgotPasswordConfirmation");
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new ResetPasswordViewModel { UserId = userId, Token = token };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = await _accountService.ResetPasswordAsync(model.UserId, model.Token, model.NewPassword);

            if (success)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            ModelState.AddModelError(string.Empty, "Resetowanie hasła nie powiodło sie. Sprawdź poprawność adresu e-mail.");
            return View(model);
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}
