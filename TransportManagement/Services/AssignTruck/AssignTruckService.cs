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

        public void AssignmentTruck(int truckId, int driverId)
        {
            var truck = _context.Trucks.FirstOrDefault(x => x.Id == truckId);
            var driver = _context.Drivers.FirstOrDefault(x => x.Id == driverId);
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
            _context.SaveChanges();
        }

        public void DeleteAssignment(int id)
        {
            var assign = _context.AssignTrucks.FirstOrDefault(x => x.Id == id);

            if(assign != null)
            {
                var truck = _context.Trucks.FirstOrDefault(x=>x.Id == assign.TruckId);
                var driver = _context.Drivers.FirstOrDefault(x=>x.Id == assign.DriverId);
                if(truck != null && driver != null)
                {
                    truck.IsAssignedDriver = false;
                    driver.IsAssignedTruck = false;
                }
                _context.AssignTrucks.Remove(assign);
                _context.SaveChanges();
            }
        }

        public AssignTruckModel GetAssignment(int id)
        {
            var assign = _context.AssignTrucks.FirstOrDefault(x => x.Id == id);
            return assign ?? new AssignTruckModel();
        }

        public List<AssignTruckModel> GetAssignments()
        {
            return _context.AssignTrucks.ToList();
        }

        public void ReturTruck(int id)
        {
            var assign = _context.AssignTrucks.FirstOrDefault(x=> x.Id == id);
            if(assign != null)
            {
                assign.IsReturned = true;
                assign.ReturnDate = DateTime.Now;
                var truck = _context.Trucks.FirstOrDefault(x => x.Id == assign.TruckId);
                var driver = _context.Drivers.FirstOrDefault(x => x.Id == assign.DriverId);
                if (truck != null && driver != null)
                {
                    truck.IsAssignedDriver = false;
                    driver.IsAssignedTruck = false;
                }
                _context.SaveChanges();
            }
        }
    }
}
