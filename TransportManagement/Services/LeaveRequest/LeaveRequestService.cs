using Microsoft.EntityFrameworkCore;
using TransportManagement.Models.LeaveRequests;

namespace TransportManagement.Services.LeaveRequest
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly TransportManagementDbContext _context;

        public LeaveRequestService(TransportManagementDbContext context)
        {
            _context = context;
        }
        public async Task<bool> ApproveLeaveRequest(int id)
        {
            var request = await _context.LeaveRequests.FindAsync(id);
            if (request == null) return false;

            request.Status = LeaveStatus.Approved;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<LeaveRequestModel>> GetAllRequests()
        {
            return await _context.LeaveRequests.ToListAsync();
        }

        public async Task<LeaveRequestModel?> GetRequestById(int id)
        {
            return await _context.LeaveRequests.FindAsync(id);
        }

        public async Task<List<LeaveRequestModel>> GetUserRequest(string userId)
        {
            return await _context.LeaveRequests.Where(r => r.UserId == userId).ToListAsync();
        }

        public async Task<bool> RejectLeaveRequest(int id, string adminComment)
        {
            var request = await _context.LeaveRequests.FindAsync(id);
            if(request == null) return false;

            request.Status = LeaveStatus.Reject;
            request.AdminComment = adminComment;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SubmitLeaveRequest(string userId, DateTime startDate, DateTime endDate)
        {
            bool alreadyExists = await _context.LeaveRequests.AnyAsync(r => r.UserId == userId &&
                ((startDate >= r.StartDate && startDate <= r.EndDate) ||
                (endDate >= r.StartDate && endDate <= r.EndDate) ||
                (startDate <= r.StartDate && endDate >= r.EndDate)
                    ));

            if (alreadyExists)
            {
                return false;
            }

            var leaveRequest = new LeaveRequestModel
            {
                UserId = userId,
                StartDate = startDate,
                EndDate = endDate,
                Status = LeaveStatus.Pending
            };

            _context.LeaveRequests.Add(leaveRequest);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}