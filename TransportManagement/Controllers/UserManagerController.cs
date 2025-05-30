﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportManagement.Models.User;
using TransportManagement.Services.User.ManageUser;

namespace TransportManagement.Controllers
{
    public class UserManagerController : Controller
    {
        private readonly IUserManagerService _userManagerService;

        public UserManagerController(IUserManagerService userManagerService)
        {
            _userManagerService = userManagerService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser()
        {
            var users = await _userManagerService.GetUserAsync();
            var userViewModel = new UserViewModel
            {
                Users = users
            };
            return View(userViewModel);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateThisUser(ApplicationUser model)
        {
            if (model.Email != null)
            {
                await _userManagerService.UpdateUserAsync(model.Email, model.FirstName, model.LastName, model.DateOfBirth, model.PhoneNumber, model.Address, model.Experience, model.Salary);
                return RedirectToAction("ManageRole", "Role");
            }
            TempData["message"] = "Popraw dane.";
            return RedirectToAction("ManageRole", "Role");
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> DeleteUser()
        {
            var users = await _userManagerService.GetUserAsync();
            var userViewModel = new UserViewModel
            {
                Users = users
            };
            return View(userViewModel);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteThisUser(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["Message"] = "Email użytkownika jest wymagany.";
                return RedirectToAction("ManageRole", "Role");
            }

            var result = await _userManagerService.DeleteUserAsync(email);
            if (result)
            {
                TempData["Message"] = $"Użytkownik '{email}' został usunięty.";
            }
            else
            {
                TempData["Message"] = $"Nie udało się usunąć użytkownika '{email}'.";
            }
            return RedirectToAction("ManageRole", "Role");
        }
    }
}
