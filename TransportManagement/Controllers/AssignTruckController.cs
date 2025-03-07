using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TransportManagement.Models.AssignTruck;
using TransportManagement.Services.AssignTruck;
using TransportManagement.Services.Driver;
using TransportManagement.Services.Truck;

namespace TransportManagement.Controllers
{
    [Authorize(Roles = "Admin, Dispatcher")]
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

        public async Task<IActionResult> Index()
        {
            var model = new AssignTruckViewModel()
            {
                AssignTrucks = await _assignTruckService.GetAssignments(),
                Trucks = await _truckService.GetTrucks(),
                Drivers = await _driverService.GetDrivers()
            };
            return View(model);
        }

        public async Task<IActionResult> AssignmentTrucks()
        {
            var model = new AssignTruckViewModel()
            {
                AssignTrucks = await _assignTruckService.GetAssignments(),
                Trucks = await _truckService.GetTrucks(),
                Drivers = await _driverService.GetDrivers()
            };
            return View(model);
        }

        public async Task<IActionResult> AssignmentThisTruck(int truckId, int driverId)
        {
            try
            {
                await _assignTruckService.AssignmentTruck(truckId, driverId);
                return RedirectToAction("Index");
            }
            catch (ArgumentException e)
            {
                TempData["message"] = e.Message;
            }
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> DeleteAssignment()
        {
            var model = new AssignTruckViewModel()
            {
                AssignTrucks = await _assignTruckService.GetAssignments(),
                Trucks = await _truckService.GetTrucks(),
                Drivers = await _driverService.GetDrivers()
            };
            return View(model);
        }

        public async Task<IActionResult> DeleteThisAssignment(int id)
        {
            try
            {
                await _assignTruckService.DeleteAssignment(id);
                return RedirectToAction("Index");
            }
            catch (ArgumentException e)
            {
                TempData["message"] = e.Message;
            }
            return RedirectToAction("Index");

            /*
            var items = _assignTruckService.GetAssignments().Count();
            if(id != 0)
            {
                _assignTruckService.DeleteAssignment(id);
                return RedirectToAction("Index");
            }

            TempData["message"] = "Popraw dane.";
            return RedirectToAction("Index");
            */
        }

        public async Task<IActionResult> ReturnAssignment()
        {
            var model = new AssignTruckViewModel()
            {
                AssignTrucks = await _assignTruckService.GetAssignments(),
                Trucks = await _truckService.GetTrucks(),
                Drivers = await _driverService.GetDrivers()
            };
            return View(model);
        }

        public async Task<IActionResult> ReturnThisAssignmnet(int id)
        {
            try
            {
                await _assignTruckService.DeleteAssignment(id);
                return RedirectToAction("Index");
            }
            catch (ArgumentException e)
            {
                TempData["message"] = e.Message;
            }
            return RedirectToAction("Index");

            /*
            if (id != 0)
            {
                _assignTruckService.ReturnTruck(id);
                return RedirectToAction("Index");
            }

            TempData["message"] = "Popraw dane.";
            return RedirectToAction("Index");
            */
        }
    }
}
