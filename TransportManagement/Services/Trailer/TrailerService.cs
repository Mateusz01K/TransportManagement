using Microsoft.EntityFrameworkCore;
using TransportManagement.Models.Trailer;

namespace TransportManagement.Services.Trailer
{
    public class TrailerService : ITrailerService
    {
        private readonly TransportManagementDbContext _context;
        public TrailerService(TransportManagementDbContext context)
        {
            _context = context;
        }
        public async Task AddTrailer(string Brand, string Model, string Type, float Mileage, float MaxLoad, string LicensePlate, int YearOfProduction)
        {
            var trailer = new TrailerModel
            {
                Brand= Brand,
                Model= Model,
                Type= Type,
                Mileage= Mileage,
                MaxLoad= MaxLoad,
                LicensePlate= LicensePlate,
                YearOfProduction= YearOfProduction
            };
            _context.Trailers.Add(trailer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTrailer(int id)
        {
            var trailer = await _context.Trailers.FirstOrDefaultAsync(x => x.Id == id);
            if (trailer != null)
            {
                _context.Trailers.Remove(trailer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TrailerModel> GetTrailer(int id)
        {
            return await _context.Trailers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<TrailerModel>> GetTrailers()
        {
            return await _context.Trailers.ToListAsync();
        }

        public async Task UpdateTrailer(int id, string Brand, string Model, string Type, float Mileage, float MaxLoad, string LicensePlate, int YearOfProduction)
        {
            var trailer = await _context.Trailers.FirstOrDefaultAsync(x => x.Id == id);
            if (trailer != null)
            {
                trailer.Brand = !string.IsNullOrEmpty(Brand) ? Brand : trailer.Brand;
                trailer.Model = !string.IsNullOrEmpty(Model) ? Model : trailer.Model;
                trailer.Type = !string.IsNullOrEmpty(Type) ? Type : trailer.Type;
                trailer.Mileage = Mileage >= 0 ? Mileage : trailer.Mileage;
                trailer.MaxLoad = MaxLoad >= 0 ? MaxLoad : trailer.MaxLoad;
                trailer.LicensePlate = !string.IsNullOrEmpty(LicensePlate) ? LicensePlate : trailer.LicensePlate;
                trailer.YearOfProduction = YearOfProduction > 1900 ? YearOfProduction : trailer.YearOfProduction;
                await _context.SaveChangesAsync();
            }
        }
    }
}
