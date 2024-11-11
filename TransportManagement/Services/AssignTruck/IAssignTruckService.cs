using TransportManagement.Models.AssignTruck;

namespace TransportManagement.Services.AssignTruck
{
    public interface IAssignTruckService
    {
        public void AssignmentTruck(int truckId, int driverId);
        public AssignTruckModel GetAssignment(int id);
        public List<AssignTruckModel> GetAssignments();
        public void DeleteAssignment(int id);
        public void ReturTruck(int id);
    }
}
