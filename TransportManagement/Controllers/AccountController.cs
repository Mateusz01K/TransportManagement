﻿using Microsoft.AspNetCore.Authorization;
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
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
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


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageRole()
        {
            var users = await _accountService.GetUserAsync();
            var userViewModel = new UserViewModel
            {
                Users = users
            };
            return View(userViewModel);
        }



        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> AssignRole()
        {
            var users = await _accountService.GetUserAsync();
            var userViewModel = new UserViewModel
            {
                Users = users
            };
            return View(userViewModel);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AssignRole(string email, string roleName)
        {
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(roleName))
            {
                TempData["Message"] = "Użytkownik/Rola są wymagane.";
                return RedirectToAction("ManageRole");
            }

            var result = await _accountService.AssignRoleAsync(email, roleName);
            if (result)
            {
                TempData["Message"] = $"Rola '{roleName}' została dodana użytkownikowi {email}.";
            }
            else
            {
                TempData["Message"] = $"Nie udało się dodać roli '{roleName}' użytkownikowi {email}";
            }
            return RedirectToAction("ManageRole");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> UnAssignRole()
        {
            var users = await _accountService.GetUserAsync();
            var userViewModel = new UserViewModel
            {
                Users = users
            };
            return View(userViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UnAssignRole(string email, string roleName)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(roleName))
            {
                TempData["Message"] = "Użytkownik/Rola są wymagane.";
                return RedirectToAction("ManageRole");
            }

            var result = await _accountService.AssignRoleAsync(email, roleName);
            if (result)
            {
                TempData["Message"] = $"Rola '{roleName}' została usunięta użytkownikowi {email}.";
            }
            else
            {
                TempData["Message"] = $"Nie udało się usunąć roli '{roleName}' użytkownikowi {email}";
            }
            return RedirectToAction("ManageRole");
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}