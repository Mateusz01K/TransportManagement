using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TransportManagement.Models.LeaveRequests;
using TransportManagement.Services.LeaveRequest;
using TransportManagement.Services.User;

namespace TransportManagement.Controllers
{
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly IUserService _userService;

        public LeaveRequestController(ILeaveRequestService leaveRequestService, IUserService userService)
        {
            _leaveRequestService = leaveRequestService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var request = await _leaveRequestService.GetAllRequests();
            var userEmails = await _userService.GetAllUsersEmails();
            var requestViewModel = new LeaveRequestViewModel
            {
                LeaveRequests = request,
                UsersEmails = userEmails
            };
            return View(requestViewModel);
        }

        public async Task<IActionResult> UserRequest()
        {
            var user = await _userService.GetCurrentUser(User);
            if(user==null)
            {
                return Unauthorized();
            }

            var request = await _leaveRequestService.GetUserRequest(user.Id);
            var requestViewModel = new LeaveRequestViewModel
            {
                LeaveRequests = request
            };
            return View(requestViewModel);
        }

        public async Task<IActionResult> GetAllRequests()
        {
            var request = await _leaveRequestService.GetAllRequests();
            return Ok(request);
        }

        public async Task<IActionResult> GetUserRequest(string userId)
        {
            var request = await _leaveRequestService.GetUserRequest(userId);
            return Ok(request);
        }

        [HttpGet]
        public IActionResult SubmitLeaveRequest()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SubmitLeaveRequest(LeaveRequestModel model)
        {
            var success = await _leaveRequestService.SubmitLeaveRequest(model.UserId, model.StartDate, model.EndDate);
            if (!success)
            {
                TempData["message"] = "Nie można złożyć wniosku na te same dni.";
                return RedirectToAction("UserRequest");
            }
            return RedirectToAction("UserRequest");
        }

        public async Task<IActionResult> ApproveLeaveRequest(int id)
        {
            var success = await _leaveRequestService.ApproveLeaveRequest(id);
            if (!success)
            {
                TempData["message"] = "Nie poprawne dane.";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RejectLeaveRequest(int id, string adminComment)
        {
            var success = await _leaveRequestService.RejectLeaveRequest(id, adminComment);
            if (!success)
            {
                TempData["message"] = "Nie poprawne dane.";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ArchivedLeaveRequestsForUser()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["message"] = "Nie można wyświetlić archiwum.";
                return RedirectToAction("Index", "Home");
            }
            var archivedRequests = await _leaveRequestService.GetArchivedLeaveRequestsForUsers(userId);
            var model = new LeaveRequestViewModel
            {
                LeaveRequests = archivedRequests
            };
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> ArchivedLeaveRequests()
        {
            var userEmails = await _userService.GetAllUsersEmails();
            var archivedRequests = await _leaveRequestService.GetArchivedLeaveRequests();
            var model = new LeaveRequestViewModel
            {
                LeaveRequests = archivedRequests,
                UsersEmails = userEmails
            };
            return View(model);
        }
    }
}
