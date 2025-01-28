using Microsoft.AspNetCore.Mvc;
using TransportManagement.Models.Trailer;
using TransportManagement.Services.Trailer;

namespace TransportManagement.Controllers
{
    public class TrailerController : Controller
    {
        private readonly ITrailerService _trailerService;
        public TrailerController(ILogger<TrailerController> logger, ITrailerService trailerService)
        {
            _trailerService = trailerService;
        }
        public IActionResult Index()
        {
            var model = new TrailerViewModel()
            {
                Trailers = _trailerService.GetTrailers()
            };
            return View(model);
        }

        public IActionResult AddTrailer()
        {
            return View();
        }

        public IActionResult AddNewTrailer(string Brand, string Model, string Type, float Mileage, float MaxLoad, string LicensePlate, int YearOfProduction)
        {
            if (string.IsNullOrEmpty(Brand) || string.IsNullOrEmpty(Model) || string.IsNullOrEmpty(Type) || (Mileage == 0)
                || (MaxLoad < -1) || string.IsNullOrEmpty(LicensePlate) || (YearOfProduction < -1))
            {
                TempData["message"] = "Popraw dane.";
                return RedirectToAction("Index");
            }
            _trailerService.AddTrailer(Brand, Model, Type, Mileage, MaxLoad, LicensePlate, YearOfProduction);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteTrailer()
        {
            var model = new TrailerViewModel()
            {
                Trailers = _trailerService.GetTrailers()
            };
            return View(model);
        }

        public IActionResult DeleteThisTrailer(int id)
        {
            if (id != 0)
            {
                _trailerService.DeleteTrailer(id);
                return RedirectToAction("Index");
            }
            TempData["message"] = "Popraw dane.";
            return RedirectToAction("Index");
        }

        public IActionResult UpdateTrailer()
        {
            var model = new TrailerViewModel()
            {
                Trailers = _trailerService.GetTrailers()
            };
            return View(model);
        }

        public IActionResult UpdateThisTrailer(int id, string Brand, string Model, string Type, float Mileage, float MaxLoad, string LicensePlate, int YearOfProduction)
        {
            var items = _trailerService.GetTrailers().Count();
            if (id != 0)// && Brand != "" && Model != "" && Type != "" && Mileage < -1 && LicensePlate != "" && YearOfProduction < -1)
            {
                _trailerService.UpdateTrailer(id, Brand, Model, Type, Mileage, MaxLoad, LicensePlate, YearOfProduction);
                return RedirectToAction("Index");
            }
            TempData["message"] = "Popraw dane.";
            return RedirectToAction("Index");
        }
    }
}
