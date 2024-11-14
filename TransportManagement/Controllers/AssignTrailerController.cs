using Microsoft.AspNetCore.Mvc;
using TransportManagement.Models.AssignTrailer;
using TransportManagement.Services.AssignTrailer;
using TransportManagement.Services.Trailer;
using TransportManagement.Services.Truck;

namespace TransportManagement.Controllers
{
    public class AssignTrailerController : Controller
    {
        private readonly IAssignTrailerService _assignTrailerService;
        private readonly ITruckService _truckService;
        private readonly ITrailerService _trailerService;

        public AssignTrailerController(IAssignTrailerService assignTrailerService, ITruckService truckService, ITrailerService trailerService)
        {
            _assignTrailerService = assignTrailerService;
            _truckService = truckService;
            _trailerService = trailerService;
        }

        public IActionResult Index()
        {
            var model = new AssignTrailerViewModel()
            {
                AssignTrailer = _assignTrailerService.GetAssignments(),
                Truck = _truckService.GetTrucks(),
                Trailer = _trailerService.GetTrailers()
            };
            return View(model);
        }

        public IActionResult AssignmentTrailers()
        {
            var model = new AssignTrailerViewModel()
            {
                AssignTrailer = _assignTrailerService.GetAssignments(),
                Truck = _truckService.GetTrucks(),
                Trailer = _trailerService.GetTrailers()
            };
            return View(model);
        }

        public IActionResult AssignmentThisTrailer(int truckId, int trailerId)
        {
            if (truckId != 0 && trailerId != 0)
            {
                _assignTrailerService.AssignmentTrailer(truckId, trailerId);
                return RedirectToAction("Index");
            }

            TempData["message"] = "Popraw dane.";
            return RedirectToAction("Index");
        }

        public IActionResult DeleteAssignment()
        {
            var model = new AssignTrailerViewModel()
            {
                AssignTrailer = _assignTrailerService.GetAssignments(),
                Truck = _truckService.GetTrucks(),
                Trailer = _trailerService.GetTrailers()
            };
            return View(model);
        }

        public IActionResult DeleteThisAssignment(int id)
        {
            var items = _assignTrailerService.GetAssignments().Count();
            if (id != 0)
            {
                _assignTrailerService.DeleteAssignment(id);
                return RedirectToAction("Index");
            }

            TempData["message"] = "Popraw dane.";
            return RedirectToAction("Index");
        }

        public IActionResult ReturnAssignment()
        {
            var model = new AssignTrailerViewModel()
            {
                AssignTrailer = _assignTrailerService.GetAssignments(),
                Truck = _truckService.GetTrucks(),
                Trailer = _trailerService.GetTrailers()
            };
            return View(model);
        }

        public IActionResult ReturnThisAssignmnet(int id)
        {
            if (id != 0)
            {
                _assignTrailerService.ReturnTrailer(id);
                return RedirectToAction("Index");
            }

            TempData["message"] = "Popraw dane.";
            return RedirectToAction("Index");
        }
    }
}
