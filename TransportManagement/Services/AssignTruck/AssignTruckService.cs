using Microsoft.EntityFrameworkCore;
using TransportManagement.Models.AssignTruck;

namespace TransportManagement.Services.AssignTruck
{
    public class AssignTruckService : IAssignTruckService
    {
        private readonly TransportManagementDbContext _context;
        public AssignTruckService(TransportManagementDbContext context)
        {
            _context = context;
        }

        public async Task AssignmentTruck(int truckId, int driverId)
        {
            var truck = await _context.Trucks.FirstOrDefaultAsync(x => x.Id == truckId);
            var driver = await _context.Drivers.FirstOrDefaultAsync(x => x.Id == driverId);
            if(truck == null || driver == null)
            {
                throw new ArgumentException("Ciezarowka lub kierowca nie istnieja.");
            }
            else if(truck.IsAssignedDriver == true || driver.IsAssignedTruck == true)
            {
                throw new ArgumentException("Ciezarowka lub kierowca sa juz przypisani.");
            }

            truck.IsAssignedDriver = true;
            driver.IsAssignedTruck = true;
            _context.AssignTrucks.Add(new AssignTruckModel
            {
                Truck = truck,
                Driver = driver,
                AssignmentDate = DateTime.Now,
                ReturnDate = DateTime.Now.AddDays(365)
            });
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAssignment(int id)
        {
            var assign = await _context.AssignTrucks.FirstOrDefaultAsync(x => x.Id == id);

            if (assign == null)
            {
                throw new ArgumentException("Ciezarowka lub kierowca nie istnieja.");
            }
            else if (assign.IsReturned)
            {
                throw new ArgumentException("Ciezarowka lub kierowca sa juz przypisani.");
            }

            var truck = await _context.Trucks.FirstOrDefaultAsync(x => x.Id == assign.TruckId);
            var driver = await _context.Drivers.FirstOrDefaultAsync(x => x.Id == assign.DriverId);
            if (truck != null && driver != null)
            {
                truck.IsAssignedDriver = false;
                driver.IsAssignedTruck = false;
            }
            _context.AssignTrucks.Remove(assign);
            await _context.SaveChangesAsync();
        }

        public async Task<AssignTruckModel> GetAssignment(int id)
        {
            return await _context.AssignTrucks.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<AssignTruckModel>> GetAssignments()
        {
            return await _context.AssignTrucks.ToListAsync();
        }

        public async Task ReturnTruck(int id)
        {
            var assign = await _context.AssignTrucks.FirstOrDefaultAsync(x=> x.Id == id);

            if (assign == null)
            {
                throw new ArgumentException("Ciezarowka lub kierowca nie istnieja.");
            }
            else if (assign.IsReturned == true)
            {
                throw new ArgumentException("Ciezarowka lub kierowca nie sa przypisani.");
            }

            assign.IsReturned = true;
            assign.ReturnDate = DateTime.Now;
            var truck = await _context.Trucks.FirstOrDefaultAsync(x => x.Id == assign.TruckId);
            var driver = await _context.Drivers.FirstOrDefaultAsync(x => x.Id == assign.DriverId);
            if (truck != null && driver != null)
            {
                truck.IsAssignedDriver = false;
                driver.IsAssignedTruck = false;
            }
            await _context.SaveChangesAsync();
        }
    }
}
