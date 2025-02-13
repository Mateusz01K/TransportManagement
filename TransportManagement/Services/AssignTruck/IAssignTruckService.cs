using TransportManagement.Models.AssignTruck;

namespace TransportManagement.Services.AssignTruck
{
    public interface IAssignTruckService
    {
        Task AssignmentTruck(int truckId, int driverId);
        Task<AssignTruckModel> GetAssignment(int id);
        Task<List<AssignTruckModel>> GetAssignments();
        Task DeleteAssignment(int id);
        Task ReturnTruck(int id);
    }
}
