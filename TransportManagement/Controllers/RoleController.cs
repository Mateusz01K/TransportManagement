using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TransportManagement.Models.User;
using TransportManagement.Services.User.ManageUser;
using TransportManagement.Services.User.RoleService;

namespace TransportManagement.Controllers
{
    public class RoleController : Controller
    {

        private readonly IRoleService _roleService;
        private readonly IUserManagerService _userManagerService;

        public RoleController(IRoleService roleService, IUserManagerService userManagerService)
        {
            _roleService = roleService;
            _userManagerService = userManagerService;
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageRole()
        {
            var users = await _userManagerService.GetUserAsync();
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
            var users = await _userManagerService.GetUserAsync();
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
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(roleName))
            {
                TempData["Message"] = "Użytkownik/Rola są wymagane.";
                return RedirectToAction("ManageRole");
            }

            var result = await _roleService.AssignRoleAsync(email, roleName);
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
            var users = await _userManagerService.GetUserAsync();
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

            var result = await _roleService.UnAssignRoleAsync(email, roleName);
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
    }
}
