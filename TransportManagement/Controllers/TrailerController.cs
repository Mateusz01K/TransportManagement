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
        public async Task<IActionResult> Index()
        {
            var trailers = await _trailerService.GetTrailers();
            var trailerViewModel = new TrailerViewModel()
            {
                Trailers = trailers
            };
            return View(trailerViewModel);
        }

        public IActionResult AddTrailer()
        {
            return View();
        }

        public async Task<IActionResult> AddNewTrailer(string Brand, string Model, string Type, float Mileage, float MaxLoad, string LicensePlate, int YearOfProduction)
        {
            if (string.IsNullOrEmpty(Brand) || string.IsNullOrEmpty(Model) || string.IsNullOrEmpty(Type) || (Mileage == 0)
                || (MaxLoad <= 0) || string.IsNullOrEmpty(LicensePlate) || (YearOfProduction < 1900))
            {
                TempData["message"] = "Popraw dane.";
                return RedirectToAction("Index");
            }
            await _trailerService.AddTrailer(Brand, Model, Type, Mileage, MaxLoad, LicensePlate, YearOfProduction);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteTrailer()
        {
            var trailers = await _trailerService.GetTrailers();
            var trailerViewModel = new TrailerViewModel()
            {
                Trailers = trailers
            };
            return View(trailerViewModel);
        }

        public async Task<IActionResult> DeleteThisTrailer(int id)
        {
            if (id != 0)
            {
                await _trailerService.DeleteTrailer(id);
                return RedirectToAction("Index");
            }
            TempData["message"] = "Popraw dane.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateTrailer()
        {
            var trailers = await _trailerService.GetTrailers();
            var trailerViewModel = new TrailerViewModel()
            {
                Trailers = trailers
            };
            return View(trailerViewModel);
        }

        public async Task<IActionResult> UpdateThisTrailer(int id, string Brand, string Model, string Type, float Mileage, float MaxLoad, string LicensePlate, int YearOfProduction)
        {
            //var items = _trailerService.GetTrailers();
            if (id != 0)
            {
                await _trailerService.UpdateTrailer(id, Brand, Model, Type, Mileage, MaxLoad, LicensePlate, YearOfProduction);
                return RedirectToAction("Index");
            }
            TempData["message"] = "Popraw dane.";
            return RedirectToAction("Index");
        }
    }
}
