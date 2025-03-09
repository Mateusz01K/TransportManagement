using Moq;
using TransportManagement.Models.Trailer;
using TransportManagement.Services.Trailer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TransportManagement.Tests
{
    public class TrailerServiceTests
    {
        private readonly TransportManagementDbContext _context;
        private readonly TrailerService _service;

        public TrailerServiceTests()
        {
            // Dynamiczna nazwa bazy danych dla każdego testu
            var options = new DbContextOptionsBuilder<TransportManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Używamy GUID, aby każdemu testowi przypisać unikalną bazę
                .Options;

            _context = new TransportManagementDbContext(options);
            _service = new TrailerService(_context);
        }

        [Fact]
        public async Task AddTrailer_Adds_New_Trailer_To_Db()
        {
            var brand = "Brand1";
            var model = "Model1";
            var type = "Type1";
            var mileage = 1000f;
            var maxLoad = 2000f;
            var licensePlate = "ABC123";
            var yearOfProduction = 2010;

            await _service.AddTrailer(brand, model, type, mileage, maxLoad, licensePlate, yearOfProduction);

            var trailers = await _context.Trailers.ToListAsync();
            Assert.Single(trailers);
            var trailer = trailers.First();
            Assert.Equal(brand, trailer.Brand);
            Assert.Equal(model, trailer.Model);
            Assert.Equal(type, trailer.Type);
            Assert.Equal(mileage, trailer.Mileage);
            Assert.Equal(maxLoad, trailer.MaxLoad);
            Assert.Equal(licensePlate, trailer.LicensePlate);
            Assert.Equal(yearOfProduction, trailer.YearOfProduction);
        }

        [Fact]
        public async Task DeleteTrailer_Removes_Trailer_From_Db()
        {
            var trailer = new TrailerModel
            {
                Id = 1,
                Brand = "Brand1",
                Model = "Model1",
                Type = "Type1",
                Mileage = 1000f,
                MaxLoad = 2000f,
                LicensePlate = "ABC123",
                YearOfProduction = 2010
            };
            _context.Trailers.Add(trailer);
            await _context.SaveChangesAsync();

            await _service.DeleteTrailer(1);

            var trailers = await _context.Trailers.ToListAsync();
            Assert.Empty(trailers);
        }

        [Fact]
        public async Task GetTrailer_Returns_Trailer_When_Exists()
        {
            var trailer = new TrailerModel
            {
                Id = 1,
                Brand = "Brand1",
                Model = "Model1",
                Type = "Type1",
                Mileage = 1000f,
                MaxLoad = 2000f,
                LicensePlate = "ABC123",
                YearOfProduction = 2010
            };
            _context.Trailers.Add(trailer);
            await _context.SaveChangesAsync();

            var result = await _service.GetTrailer(1);

            Assert.NotNull(result);
            Assert.Equal(trailer.Id, result.Id);
            Assert.Equal(trailer.Brand, result.Brand);
            Assert.Equal(trailer.Model, result.Model);
        }

        [Fact]
        public async Task GetTrailers_Returns_All_Trailers()
        {
            var trailers = new List<TrailerModel>
        {
            new TrailerModel { Id = 1, Brand = "Brand1", Model = "Model1", Type = "Type1", Mileage = 1000f, MaxLoad = 2000f, LicensePlate = "ABC123", YearOfProduction = 2010 },
            new TrailerModel { Id = 2, Brand = "Brand2", Model = "Model2", Type = "Type2", Mileage = 1500f, MaxLoad = 2500f, LicensePlate = "XYZ456", YearOfProduction = 2015 }
        };
            _context.Trailers.AddRange(trailers);
            await _context.SaveChangesAsync();

            var result = await _service.GetTrailers();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task UpdateTrailer_Updates_Trailer_When_Exists()
        {
            var trailer = new TrailerModel
            {
                Id = 1,
                Brand = "Brand1",
                Model = "Model1",
                Type = "Type1",
                Mileage = 1000f,
                MaxLoad = 2000f,
                LicensePlate = "ABC123",
                YearOfProduction = 2010
            };
            _context.Trailers.Add(trailer);
            await _context.SaveChangesAsync();

            var newBrand = "UpdatedBrand";
            await _service.UpdateTrailer(1, newBrand, trailer.Model, trailer.Type, trailer.Mileage, trailer.MaxLoad, trailer.LicensePlate, trailer.YearOfProduction);

            var updatedTrailer = await _context.Trailers.FirstOrDefaultAsync(x => x.Id == 1);
            Assert.Equal(newBrand, updatedTrailer.Brand);
        }
    }
}