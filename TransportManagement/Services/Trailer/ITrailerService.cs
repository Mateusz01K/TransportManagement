using TransportManagement.Models.Trailer;

namespace TransportManagement.Services.Trailer
{
    public interface ITrailerService
    {
        Task<List<TrailerModel>> GetTrailers();
        Task<TrailerModel> GetTrailer(int id);
        Task AddTrailer(string Brand, string Model, string Type, float Mileage, float MaxLoad, string LicensePlate , int YearOfProduction);
        Task UpdateTrailer(int id, string Brand, string Model, string Type, float Mileage, float MaxLoad, string LicensePlate , int YearOfProduction);
        Task DeleteTrailer(int id);
    }
}
