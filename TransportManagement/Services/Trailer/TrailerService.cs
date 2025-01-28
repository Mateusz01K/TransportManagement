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
        public void AddTrailer(string Brand, string Model, string Type, float Mileage, float MaxLoad, string LicensePlate, int YearOfProduction)
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
            _context.SaveChanges();
        }

        public void DeleteTrailer(int id)
        {
            var trailer = _context.Trailers.FirstOrDefault(x => x.Id == id);
            if (trailer != null)
            {
                _context.Trailers.Remove(trailer);
                _context.SaveChanges();
            }
        }

        public TrailerModel GetTrailer(int id)
        {
            var trailer = _context.Trailers.FirstOrDefault(x => x.Id == id);
            return trailer ?? new TrailerModel();
        }

        public List<TrailerModel> GetTrailers()
        {
            return _context.Trailers.ToList();
        }

        public void UpdateTrailer(int id, string Brand, string Model, string Type, float Mileage, float MaxLoad, string LicensePlate, int YearOfProduction)
        {
            var trailer = _context.Trailers.FirstOrDefault(x => x.Id == id);
            if (trailer != null)
            {
                trailer.Brand = !string.IsNullOrEmpty(Brand) ? Brand : trailer.Brand;
                trailer.Model = !string.IsNullOrEmpty(Model) ? Model : trailer.Model;
                trailer.Type = !string.IsNullOrEmpty(Type) ? Type : trailer.Type;
                trailer.Mileage = Mileage >= 0 ? Mileage : trailer.Mileage;
                trailer.MaxLoad = MaxLoad >= 0 ? MaxLoad : trailer.MaxLoad;
                trailer.LicensePlate = !string.IsNullOrEmpty(LicensePlate) ? LicensePlate : trailer.LicensePlate;
                trailer.YearOfProduction = YearOfProduction >= 0 ? YearOfProduction : trailer.YearOfProduction;
                _context.SaveChanges();
            }
        }
    }
}
