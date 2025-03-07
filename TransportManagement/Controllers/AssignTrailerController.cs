using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TransportManagement.Models.AssignTrailer;
using TransportManagement.Services.AssignTrailer;
using TransportManagement.Services.Trailer;
using TransportManagement.Services.Truck;

namespace TransportManagement.Controllers
{
    [Authorize(Roles = "Admin, Dispatcher")]
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

        public async Task<IActionResult> Index()
        {
            var model = new AssignTrailerViewModel()
            {
                AssignTrailer = await _assignTrailerService.GetAssignments(),
                Truck = await _truckService.GetTrucks(),
                Trailer = await _trailerService.GetTrailers()
            };
            return View(model);
        }

        public async Task<IActionResult> AssignmentTrailers()
        {
            var model = new AssignTrailerViewModel()
            {
                AssignTrailer = await _assignTrailerService.GetAssignments(),
                Truck = await _truckService.GetTrucks(),
                Trailer = await _trailerService.GetTrailers()
            };
            return View(model);
        }

        public async Task<IActionResult> AssignmentThisTrailer(int truckId, int trailerId)
        {
            try
            {
                await _assignTrailerService.AssignmentTrailer(truckId, trailerId);
                return RedirectToAction("Index");
            }
            catch(ArgumentException e)
            {
                TempData["message"] = e.Message;
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteAssignment()
        {
            var model = new AssignTrailerViewModel()
            {
                AssignTrailer = await _assignTrailerService.GetAssignments(),
                Truck = await _truckService.GetTrucks(),
                Trailer = await _trailerService.GetTrailers()
            };
            return View(model);
        }

        public async Task<IActionResult> DeleteThisAssignment(int id)
        {

            try
            {
                await _assignTrailerService.DeleteAssignment(id);
                return RedirectToAction("Index");
            }
            catch (ArgumentException e)
            {
                TempData["message"] = e.Message;
            }
            return RedirectToAction("Index");


            /*
            var items = _assignTrailerService.GetAssignments().Count();
            
            if (id != 0)
            {
                _assignTrailerService.DeleteAssignment(id);
                return RedirectToAction("Index");
            }

            TempData["message"] = "Popraw dane.";
            return RedirectToAction("Index");
            */
        }

        public async Task<IActionResult> ReturnAssignment()
        {
            var model = new AssignTrailerViewModel()
            {
                AssignTrailer = await _assignTrailerService.GetAssignments(),
                Truck = await _truckService.GetTrucks(),
                Trailer = await _trailerService.GetTrailers()
            };
            return View(model);
        }

        public async Task<IActionResult> ReturnThisAssignmnet(int id)
        {
            try
            {
                await _assignTrailerService.ReturnTrailer(id);
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
                _assignTrailerService.ReturnTrailer(id);
                return RedirectToAction("Index");
            }

            TempData["message"] = "Popraw dane.";
            return RedirectToAction("Index");
            */
        }
    }
}
