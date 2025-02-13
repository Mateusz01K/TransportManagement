using Microsoft.EntityFrameworkCore;
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
        public async Task AssignmentTrailer(int truckId, int trailerId)
        {
            var truck = await _context.Trucks.FirstOrDefaultAsync(x => x.Id == truckId);
            var trailer = await _context.Trailers.FirstOrDefaultAsync(x => x.Id == trailerId);
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
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAssignment(int id)
        {
            var assign = await _context.AssignTrailers.FirstOrDefaultAsync(x => x.Id == id);

            if (assign == null)
            {
                throw new ArgumentException("Przypisanie nie itnieje w bazie danych.");
            }

            var truck = await _context.Trucks.FirstOrDefaultAsync(x => x.Id == assign.TruckId);
            var trailer = await _context.Trailers.FirstOrDefaultAsync(x => x.Id == assign.TrailerId);
            if (truck != null && trailer != null)
            {
                truck.IsAssignedTrailer = false;
                trailer.IsAssigned = false;
            }
            _context.AssignTrailers.Remove(assign);
            await _context.SaveChangesAsync();

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

        public async Task<AssignTrailerModel> GetAssignment(int id)
        {
            return await _context.AssignTrailers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<AssignTrailerModel>> GetAssignments()
        {
            return await _context.AssignTrailers.ToListAsync();
        }

        public async Task ReturnTrailer(int id)
        {
            var assign = await _context.AssignTrailers.FirstOrDefaultAsync(x => x.Id == id);

            if (assign == null)
            {
                throw new ArgumentException("Przypisanie nie istnieje w bazie danych.");
            }

            assign.IsReturned = true;
            assign.ReturnDate = DateTime.Now;
            var truck = await _context.Trucks.FirstOrDefaultAsync(x => x.Id == assign.TruckId);
            var trailer = await _context.Trailers.FirstOrDefaultAsync(x => x.Id == assign.TrailerId);
            if (truck != null && trailer != null)
            {
                truck.IsAssignedTrailer = false;
                trailer.IsAssigned = false;
            }
            await _context.SaveChangesAsync();


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
