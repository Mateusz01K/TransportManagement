using TransportManagement.Models.AssignTrailer;

namespace TransportManagement.Services.AssignTrailer
{
    public interface IAssignTrailerService
    {
        public void AssignmentTrailer(int truckId, int trailerId);
        public AssignTrailerModel GetAssignment(int id);
        public List<AssignTrailerModel> GetAssignments();
        public void DeleteAssignment(int id);
        public void ReturnTrailer(int id);
    }
}
