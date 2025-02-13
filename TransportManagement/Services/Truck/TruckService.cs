using Microsoft.EntityFrameworkCore;
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
        public async Task AddTruck(string Brand, string Model, int YearOfProduction, int Power, float Mileage, int Weight, string LicensePlate)
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
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTruck(int id)
        {
            var truck = await _context.Trucks.FirstOrDefaultAsync(x => x.Id == id);
            if (truck != null)
            {
                _context.Trucks.Remove(truck);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TruckModel> GetTruck(int id)
        {
            return await _context.Trucks.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<TruckModel>> GetTrucks()
        {
            return await _context.Trucks.ToListAsync();
        }

        public async Task<bool> UpdateTruck(int id, string Brand, string Model, int YearOfProduction, int Power, float Mileage, int Weight, string LicensePlate)
        {
            var truck = await _context.Trucks.FirstOrDefaultAsync(x => x.Id == id);
            if (truck != null)
            {
                truck.Brand = !string.IsNullOrEmpty(Brand) ? Brand : truck.Brand;
                truck.Model = !string.IsNullOrEmpty(Model) ? Model : truck.Model;
                truck.YearOfProduction = YearOfProduction > 1900 ? YearOfProduction : truck.YearOfProduction;
                truck.Power = Power >= 0 ? Power : truck.Power;
                truck.Mileage = Mileage >= 0 ? Mileage : truck.Mileage;
                truck.Weight = Weight >= 0 ? Weight : truck.Weight;
                truck.LicensePlate = !string.IsNullOrEmpty(LicensePlate) ? LicensePlate : truck.LicensePlate;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
