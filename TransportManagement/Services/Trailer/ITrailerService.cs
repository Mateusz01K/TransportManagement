using TransportManagement.Models.Trailer;

namespace TransportManagement.Services.Trailer
{
    public interface ITrailerService
    {
        public List<TrailerModel> GetTrailers();
        public TrailerModel GetTrailer(int id);
        public void AddTrailer(string Brand, string Model, string Type, float Mileage, float MaxLoad, string LicensePlate , int YearOfProduction);
        public void UpdateTrailer(int id, string Brand, string Model, string Type, float Mileage, float MaxLoad, string LicensePlate , int YearOfProduction);
        public void DeleteTrailer(int id);
    }
}
