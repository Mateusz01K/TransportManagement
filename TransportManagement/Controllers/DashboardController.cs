using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TransportManagement.Models.User;
using TransportManagement.Services.User.ManageUser;

namespace TransportManagement.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(UserManager<ApplicationUser> userManager)
        {
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