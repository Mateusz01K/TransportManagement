using TransportManagement.Models.Truck;

namespace TransportManagement.Services.Truck
{
    public interface ITruckService
    {
        Task<List<TruckModel>> GetTrucks();
        Task<TruckModel> GetTruck(int id);
        Task AddTruck(string Brand, string Model, int YearOfProduction, int Power, float Mileage, int Weight, string LicensePlate);
        Task<bool> UpdateTruck(int id, string Brand, string Model, int YearOfProduction, int Power, float Mileage, int Weight, string LicensePlate);
        Task DeleteTruck(int id);
    }
}
