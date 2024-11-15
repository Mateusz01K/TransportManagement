using System.Diagnostics;
using TransportManagement.Models.AssignTrailer;

namespace TransportManagement.Services.AssignTrailer
{
    public class AssignTrailerService : IAssignTrailerService
    {
        private readonly TransportManagementDbContext _context;
        public AssignTrailerService(TransportManagementDbContext context)
        {
            _context = context;
        }
        public void AssignmentTrailer(int truckId, int trailerId)
        {
            var truck = _context.Trucks.FirstOrDefault(x => x.Id == truckId);
            var trailer = _context.Trailers.FirstOrDefault(x => x.Id == trailerId);
            if (truck == null || trailer == null)
            {
                throw new ArgumentException("Ciezarowka lub naczepa nie istnieja w bazie danych.");
            }
            else if(truck.IsAssignedTrailer == true || trailer.IsAssigned == true)
            {
                throw new ArgumentException("Ciezarowka lub naczepa sa juz przypisane.");
            }

            truck.IsAssignedTrailer = true;
            trailer.IsAssigned = true;
            _context.AssignTrailers.Add(new AssignTrailerModel
            {
                Truck = truck,
                Trailer = trailer,
                AssignmentDate = DateTime.Now,
                ReturnDate = DateTime.Now.AddDays(365)
            });
            _context.SaveChanges();
        }

        public void DeleteAssignment(int id)
        {
            var assign = _context.AssignTrailers.FirstOrDefault(x => x.Id == id);

            if (assign == null)
            {
                throw new ArgumentException("Przypisanie nie itnieje w bazie danych.");
            }

            var truck = _context.Trucks.FirstOrDefault(x => x.Id == assign.TruckId);
            var trailer = _context.Trailers.FirstOrDefault(x => x.Id == assign.TrailerId);
            if (truck != null && trailer != null)
            {
                truck.IsAssignedTrailer = false;
                trailer.IsAssigned = false;
            }
            _context.AssignTrailers.Remove(assign);
            _context.SaveChanges();

            /*
            if (assign != null)
            {
                var truck = _context.Trucks.FirstOrDefault(x => x.Id == assign.TruckId);
                var trailer = _context.Trailers.FirstOrDefault(x => x.Id == assign.TrailerId);
                if (truck != null && trailer != null)
                {
                    truck.IsAssignedTrailer = false;
                    trailer.IsAssigned = false;
                }
                _context.AssignTrailers.Remove(assign);
                _context.SaveChanges();
            }
            */
        }

        public AssignTrailerModel GetAssignment(int id)
        {
            var assign = _context.AssignTrailers.FirstOrDefault(x => x.Id == id);
            return assign ?? new AssignTrailerModel();
        }

        public List<AssignTrailerModel> GetAssignments()
        {
            return _context.AssignTrailers.ToList();
        }

        public void ReturnTrailer(int id)
        {
            var assign = _context.AssignTrailers.FirstOrDefault(x => x.Id == id);

            if (assign == null)
            {
                throw new ArgumentException("Przypisanie nie istnieje w bazie danych.");
            }

            assign.IsReturned = true;
            assign.ReturnDate = DateTime.Now;
            var truck = _context.Trucks.FirstOrDefault(x => x.Id == assign.TruckId);
            var trailer = _context.Trailers.FirstOrDefault(x => x.Id == assign.TrailerId);
            if (truck != null && trailer != null)
            {
                truck.IsAssignedTrailer = false;
                trailer.IsAssigned = false;
            }
            _context.SaveChanges();


            /*
            if (assign != null)
            {
                assign.IsReturned = true;
                assign.ReturnDate = DateTime.Now;
                var truck = _context.Trucks.FirstOrDefault(x => x.Id == assign.TruckId);
                var trailer = _context.Trailers.FirstOrDefault(x => x.Id == assign.TrailerId);
                if (truck != null && trailer != null)
                {
                    truck.IsAssignedTrailer = false;
                    trailer.IsAssigned = false;
                }
                _context.SaveChanges();
            }
            */
        }
    }
}
