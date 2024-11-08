using TransportManagement.Models.Truck;

namespace TransportManagement.Services.Truck
{
    public class TruckService : ITruckService
    {
        private readonly TransportManagementDbContext _context;

        public TruckService(TransportManagementDbContext context)
        {
            _context = context;
        }
        public void AddTruck(string Brand, string Model, int YearOfProduction, string Power, float Mileage, int Weight, string LicensePlate)
        {
            var truck = new TruckModel
            {
                Brand = Brand,
                Model = Model,
                YearOfProduction = YearOfProduction,
                Power = Power,
                Mileage = Mileage,
                Weight = Weight,
                LicensePlate = LicensePlate,
            };
            _context.Trucks.Add(truck);
            _context.SaveChanges();
        }

        public void DeleteTruck(int id)
        {
            var truck = _context.Trucks.FirstOrDefault(x => x.Id == id);
            if (truck != null)
            {
                _context.Trucks.Remove(truck);
                _context.SaveChanges();
            }
        }

        public TruckModel GetTruck(int id)
        {
            var truck = _context.Trucks.FirstOrDefault(x => x.Id == id);
            return truck ?? new TruckModel();
        }

        public List<TruckModel> GetTrucks()
        {
            return _context.Trucks.ToList();
        }

        public void UpdateTruck(int id, string Brand, string Model, int YearOfProduction, string Power, float Mileage, int Weight, string LicensePlate)
        {
            var truck = _context.Trucks.FirstOrDefault(x => x.Id == id);
            if (truck != null)
            {
                truck.Brand = Brand;
                truck.Model = Model;
                truck.YearOfProduction = YearOfProduction;
                truck.Power = Power;
                truck.Mileage = Mileage;
                truck.Weight = Weight;
                truck.LicensePlate = LicensePlate;
                _context.SaveChanges();
            }
        }
    }
}
