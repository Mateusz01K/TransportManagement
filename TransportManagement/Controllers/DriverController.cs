using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            var model = new DriverViewModel()
            {
                Drivers = _driverService.GetDrivers()
            };
            return View(model);
        }

        public IActionResult AddDriver()
        {
            return View();
        }

        public IActionResult AddNewDriver(string Name, string LastName, DateTime DateOfBirth, string PhoneNumber, string Email, string Address, int Experience)
        {
            if(string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(LastName) && (DateOfBirth < DateTime.Now) && string.IsNullOrEmpty(PhoneNumber)
                && string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Address) && (Experience < 0))
            {
                TempData["message"] = "Popraw dane.";
                return RedirectToAction("Index");
            }
            _driverService.AddDriver(Name, LastName, DateOfBirth, PhoneNumber, Email, Address, Experience);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteDriver()
        {
            var model = new DriverViewModel()
            {
                Drivers = _driverService.GetDrivers()
            };
            return View(model);
        }

        public IActionResult DeleteThisDriver(int id)
        {
            if(id != 0)
            {
                _driverService.DeleteDriver(id);
                return RedirectToAction("Index");
            }
            TempData["message"] = "Popraw dane.";
            return RedirectToAction("Index");
        }

        public IActionResult UpdateDriver()
        {
            var model = new DriverViewModel()
            {
                Drivers = _driverService.GetDrivers()
            };
            return View(model);
        }

        public IActionResult UpdateThisDriver(int id, string Name, string LastName, DateTime DateOfBirth,
                                                string PhoneNumber, string Email, string Address, int Experience)
        {
            var items = _driverService.GetDrivers().Count();
            if(id != 0 && Name != "" && LastName != "" && DateOfBirth < DateTime.Now && PhoneNumber != "" && Email != "" && Address != "" && Experience != 0)
            {
                _driverService.UpdateDriver(id, Name, LastName, DateOfBirth, PhoneNumber, Email, Address, Experience);
                return RedirectToAction("Index");
            }
            TempData["message"] = "Popraw dane.";
            return RedirectToAction("Index");
        }
    }
}
