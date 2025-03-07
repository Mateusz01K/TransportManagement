using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TransportManagement.Models.Truck;
using TransportManagement.Services.Truck;

namespace TransportManagement.Controllers
{
    public class TruckController : Controller
    {
        private readonly ITruckService _truckService;
        public TruckController(ILogger<TruckController> logger, ITruckService truckService)
        {
            _truckService = truckService;
        }

        [Authorize(Roles = "Admin, Dispatcher")]
        public async Task<IActionResult> Index()
        {
            var trailers = await _truckService.GetTrucks();
            var truckViewModel = new TruckViewModel()
            {
                Trucks = trailers
            };
            return View(truckViewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddTruck()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddNewTruck(string Brand, string Model, int YearOfProduction, int Power, float Mileage, int Weight, string LicensePlate)
        {
            if (string.IsNullOrEmpty(Brand) && string.IsNullOrEmpty(Model) && (YearOfProduction < -1) && (Power > -1)
                && (Mileage > -1) && (Weight > -1) && string.IsNullOrEmpty(LicensePlate))
            {
                TempData["message"] = "Popraw dane.";
                return RedirectToAction("Index");
            }
            await _truckService.AddTruck(Brand, Model, YearOfProduction, Power, Mileage, Weight, LicensePlate);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTruck()
        {
            var trailers = await _truckService.GetTrucks();
            var truckViewModel = new TruckViewModel()
            {
                Trucks = trailers
            };
            return View(truckViewModel);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteThisTruck(int id)
        {
            if (id != 0)
            {
                await _truckService.DeleteTruck(id);
                return RedirectToAction("Index");
            }
            TempData["message"] = "Popraw dane.";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTruck()
        {
            var trailers = await _truckService.GetTrucks();
            var truckViewModel = new TruckViewModel()
            {
                Trucks = trailers
            };
            return View(truckViewModel);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateThisTruck(int id, string Brand, string Model, int YearOfProduction, int Power, float Mileage, int Weight, string LicensePlate)
        {
            //var items = _truckService.GetTrucks();
            if (id != 0)
            {
                await _truckService.UpdateTruck(id, Brand, Model, YearOfProduction, Power, Mileage, Weight, LicensePlate);
                return RedirectToAction("Index");
            }
            TempData["message"] = "Popraw dane.";
            return RedirectToAction("Index");
        }
    }
}
