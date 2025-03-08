using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TransportManagement.Models.Drivers;
using TransportManagement.Services.Driver;

namespace TransportManagement.Controllers
{
    public class DriverController : Controller
    {
        private readonly IDriverService _driverService;
        public DriverController(ILogger<DriverController> logger, IDriverService driverService)
        {
            _driverService = driverService;
        }

        [Authorize(Roles = "Admin, Dispatcher")]
        public async Task<IActionResult> Index()
        {
            var drivers = await _driverService.GetDrivers();
            var driverViewModel = new DriverViewModel
            {
                Drivers = drivers
            };
            return View(driverViewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddDriver()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddNewDriver(string Name, string LastName, DateTime DateOfBirth, string PhoneNumber, string Email, string Address, int Experience, decimal Salary)
        {
            if(string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(LastName) || (DateOfBirth >= DateTime.Now) || string.IsNullOrEmpty(PhoneNumber)
                || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Address) || (Experience < 0) || (Salary < 0))
            {
                TempData["message"] = "Popraw dane.";
                return RedirectToAction("Index");
            }

            var result = await _driverService.AddDriver(Name, LastName, DateOfBirth, PhoneNumber, Email, Address, Experience, Salary);

            if (!result)
            {
                TempData["message"] = "Nie udało się dodać kierowcy.";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDriver()
        {
            var drivers = await _driverService.GetDrivers();
            var driverViewModel = new DriverViewModel
            {
                Drivers = drivers
            };
            return View(driverViewModel);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteThisDriver(int id)
        {
            if(id != 0)
            {
                await _driverService.DeleteDriver(id);
                return RedirectToAction("Index");
            }
            TempData["message"] = "Popraw dane.";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDriver()
        {
            var drivers = await _driverService.GetDrivers();
            var driverViewModel = new DriverViewModel
            {
                Drivers = drivers
            };
            return View(driverViewModel);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateThisDriver(int id, string Name, string LastName, DateTime DateOfBirth,
                                                string PhoneNumber, string Email, string Address, int Experience, decimal Salary)
        {
            if(id != 0)
            {
                await _driverService.UpdateDriver(id, Name, LastName, DateOfBirth, PhoneNumber, Email, Address, Experience, Salary);
                return RedirectToAction("Index");
            }
            TempData["message"] = "Popraw dane.";
            return RedirectToAction("Index");
        }
    }
}
