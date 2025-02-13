using TransportManagement.Models.AssignTrailer;

namespace TransportManagement.Services.AssignTrailer
{
    public interface IAssignTrailerService
    {
        Task AssignmentTrailer(int truckId, int trailerId);
        Task<AssignTrailerModel> GetAssignment(int id);
        Task<List<AssignTrailerModel>> GetAssignments();
        Task DeleteAssignment(int id);
        Task ReturnTrailer(int id);
    }
}
