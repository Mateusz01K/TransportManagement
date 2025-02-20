using Microsoft.AspNetCore.Mvc;
using TransportManagement.Models.User.ResetPassword;
using TransportManagement.Services.User.ResetPassword;

namespace TransportManagement.Controllers
{
    public class ResetPasswordController : Controller
    {
        private readonly IResetPasswordService _resetPasswordService;

        public ResetPasswordController(IResetPasswordService resetPasswordService)
        {
            _resetPasswordService = resetPasswordService;
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

            var resetUrlBase = Url.Action("ResetPassword", "ResetPassword", null, Request.Scheme);
            await _resetPasswordService.SendPasswordResetEmailAsync(model.Email, resetUrlBase);

            //TempData["message"] = "Otrzymasz wiadomośc e-mail.";
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

            var success = await _resetPasswordService.ResetPasswordAsync(model.UserId, model.Token, model.NewPassword);

            if (success)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            //ModelState.AddModelError(string.Empty, "Resetowanie hasła nie powiodło sie. Sprawdź poprawność adresu e-mail.");
            //TempData["message"] = "Resetowanie hasła nie powiodło sie. Sprawdź poprawność adresu e-mail.";
            return View(model);
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}
