using Microsoft.AspNetCore.Mvc;
using TransportManagement.Models.User;
using TransportManagement.Services.User;
using TransportManagement.Services.User.ManageUser;

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
                //TempData["message"] = "Rejestracja zakończona sukcesem.";
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
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction("LoginUser");
        }
    }
}