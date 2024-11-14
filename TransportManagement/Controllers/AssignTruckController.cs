using Microsoft.AspNetCore.Mvc;
using TransportManagement.Models.AssignTruck;
using TransportManagement.Services.AssignTruck;
using TransportManagement.Services.Driver;
using TransportManagement.Services.Truck;

namespace TransportManagement.Controllers
{
    public class AssignTruckController : Controller
    {
        private readonly IAssignTruckService _assignTruckService;
        private readonly ITruckService _truckService;
        private readonly IDriverService _driverService;
        private readonly TransportManagementDbContext _context;

        public AssignTruckController(IAssignTruckService assignTruckService, ITruckService truckService, IDriverService driverService,
                                        TransportManagementDbContext context)
        {
            _assignTruckService = assignTruckService;
            _truckService = truckService;
            _driverService = driverService;
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new AssignTruckViewModel()
            {
                AssignTrucks = _assignTruckService.GetAssignments(),
                Trucks = _truckService.GetTrucks(),
                Drivers = _driverService.GetDrivers()
            };
            return View(model);
        }

        public IActionResult AssignmentTrucks()
        {
            var model = new AssignTruckViewModel()
            {
                AssignTrucks = _assignTruckService.GetAssignments(),
                Trucks = _truckService.GetTrucks(),
                Drivers = _driverService.GetDrivers()
            };
            return View(model);
        }

        public IActionResult AssignmentThisTruck(int truckId, int driverId)
        {
            try
            {
                _assignTruckService.AssignmentTruck(truckId, driverId);
                return RedirectToAction("Index");
            }
            catch (ArgumentException e)
            {
                TempData["message"] = e.Message;
            }
            return RedirectToAction("Index");

        }

        public IActionResult DeleteAssignment()
        {
            var model = new AssignTruckViewModel()
            {
                AssignTrucks = _assignTruckService.GetAssignments(),
                Trucks = _truckService.GetTrucks(),
                Drivers = _driverService.GetDrivers()
            };
            return View(model);
        }

        public IActionResult DeleteThisAssignment(int id)
        {
            var items = _assignTruckService.GetAssignments().Count();
            if(id != 0)
            {
                _assignTruckService.DeleteAssignment(id);
                return RedirectToAction("Index");
            }

            TempData["message"] = "Popraw dane.";
            return RedirectToAction("Index");
        }

        public IActionResult ReturnAssignment()
        {
            var model = new AssignTruckViewModel()
            {
                AssignTrucks = _assignTruckService.GetAssignments(),
                Trucks = _truckService.GetTrucks(),
                Drivers = _driverService.GetDrivers()
            };
            return View(model);
        }

        public IActionResult ReturnThisAssignmnet(int id)
        {
            if(id != 0)
            {
                _assignTruckService.ReturTruck(id);
                return RedirectToAction("Index");
            }

            TempData["message"] = "Popraw dane.";
            return RedirectToAction("Index");
        }
    }
}
