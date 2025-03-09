using System.Security.Claims;
using TransportManagement.Models.LeaveRequests;
using TransportManagement.Models.User;

namespace TransportManagement.Services.LeaveRequest
{
    public interface ILeaveRequestService
    {
        Task<List<LeaveRequestModel>> GetAllRequests();
        Task<List<LeaveRequestModel>> GetUserRequest(string userId);
        Task<LeaveRequestModel?> GetRequestById(int id);
        Task<bool> SubmitLeaveRequest(string userId, DateTime startDate, DateTime endDate);
        Task<bool> ApproveLeaveRequest(int id);
        Task<bool> RejectLeaveRequest(int id, string adminComment);
        Task<List<LeaveRequestModel>> GetArchivedLeaveRequestsForUsers(string userId);
        Task<List<LeaveRequestModel>> GetArchivedLeaveRequests();
    }
}
