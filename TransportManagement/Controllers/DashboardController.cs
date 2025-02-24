using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TransportManagement.Models.User;
using TransportManagement.Services.User.ManageUser;

namespace TransportManagement.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUserManagerService _userManagerService;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(IUserManagerService userManagerService, UserManager<ApplicationUser> userManager)
        {
            _userManagerService = userManagerService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Admin"))
            {
                return View("AdminDashboard");
            }

            if (roles.Contains("Dispatcher"))
            {
                return View("DispatcherDashboard");
            }

            if (roles.Contains("Driver"))
            {
                return View("DriverDashboard");
            }


            return View("DefaultDashboard");
        }
    }
}