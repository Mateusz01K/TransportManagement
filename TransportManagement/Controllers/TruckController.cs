using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            var model = new TruckViewModel()
            {
                Trucks = _truckService.GetTrucks()
            };
            return View(model);
        }

        public IActionResult AddTruck()
        {
            return View();
        }

        public IActionResult AddNewTruck(string Brand, string Model, int YearOfProduction, string Power, float Mileage, int Weight, string LicensePlate)
        {
            if (string.IsNullOrEmpty(Brand) && string.IsNullOrEmpty(Model) && (YearOfProduction < -1) && string.IsNullOrEmpty(Power)
                && (Mileage < -1) && (Weight < -1) && string.IsNullOrEmpty(LicensePlate))
            {
                TempData["message"] = "Popraw dane.";
                return RedirectToAction("Index");
            }
            _truckService.AddTruck(Brand, Model, YearOfProduction, Power, Mileage, Weight, LicensePlate);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteTruck()
        {
            var model = new TruckViewModel()
            {
                Trucks = _truckService.GetTrucks()
            };
            return View(model);
        }

        public IActionResult DeleteThisTruck(int id)
        {
            if (id != 0)
            {
                _truckService.DeleteTruck(id);
                return RedirectToAction("Index");
            }
            TempData["message"] = "Popraw dane.";
            return RedirectToAction("Index");
        }

        public IActionResult UpdateTruck()
        {
            var model = new TruckViewModel()
            {
                Trucks = _truckService.GetTrucks()
            };
            return View(model);
        }

        public IActionResult UpdateThisTruck(int id, string Brand, string Model, int YearOfProduction, string Power, float Mileage, int Weight, string LicensePlate)
        {
            var items = _truckService.GetTrucks().Count();
            if (id != 0 && Brand != "" && Model != "" && YearOfProduction < -1 && Power != "" && Mileage < -1 && Weight < -1 && LicensePlate != "")
            {
                _truckService.UpdateTruck(id, Brand, Model, YearOfProduction, Power, Mileage, Weight, LicensePlate);
                return RedirectToAction("Index");
            }
            TempData["message"] = "Popraw dane.";
            return RedirectToAction("Index");
        }
    }
}
