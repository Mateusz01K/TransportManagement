using TransportManagement.Models.Truck;

namespace TransportManagement.Services.Truck
{
    public interface ITruckService
    {
        public List<TruckModel> GetTrucks();
        public TruckModel GetTruck(int id);
        public void AddTruck(string Brand, string Model, int YearOfProduction, int Power, float Mileage, int Weight, string LicensePlate);
        public void UpdateTruck(int id, string Brand, string Model, int YearOfProduction, int Power, float Mileage, int Weight, string LicensePlate);
        public void DeleteTruck(int id);
    }
}
