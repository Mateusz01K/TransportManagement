using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TransportManagement.Controllers;
using TransportManagement.Models.Trailer;
using TransportManagement.Services.Trailer;

namespace TransportManagement.Tests
{
    public class TrailerControllerTests
    {
        private readonly Mock<ITrailerService> _mockTrailerService;
        private readonly TrailerController _controller;

        public TrailerControllerTests()
        {
            _mockTrailerService = new Mock<ITrailerService>();
            _controller = new TrailerController(Mock.Of<ILogger<TrailerController>>(), _mockTrailerService.Object);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Fact]
        public async Task Index_Returns_ViewResult_With_Trailers()
        {
            var trailers = new List<TrailerModel>
            {
                new TrailerModel { Id = 1, Brand = "Brand1", Model = "Model1", Type = "Type1", Mileage = 1000, MaxLoad = 2000, LicensePlate = "ABC123", YearOfProduction = 2010 },
                new TrailerModel { Id = 2, Brand = "Brand2", Model = "Model2", Type = "Type2", Mileage = 1500, MaxLoad = 3000, LicensePlate = "DEF456", YearOfProduction = 2015 }
            };
            _mockTrailerService.Setup(service => service.GetTrailers()).ReturnsAsync(trailers);

            var result = await _controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<TrailerViewModel>(viewResult.Model);
            Assert.Equal(2, model.Trailers.Count);
        }

        [Fact]
        public async Task AddNewTrailer_Returns_RedirectToActionResult_When_Successful()
        {
            var brand = "Brand1";
            var model = "Model1";
            var type = "Type1";
            var mileage = 1000f;
            var maxLoad = 2000f;
            var licensePlate = "ABC123";
            var yearOfProduction = 2010;

            var result = await _controller.AddNewTrailer(brand, model, type, mileage, maxLoad, licensePlate, yearOfProduction);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task DeleteThisTrailer_Returns_RedirectToActionResult_When_Successful()
        {
            var trailerId = 1;

            var result = await _controller.DeleteThisTrailer(trailerId);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task UpdateThisTrailer_Returns_RedirectToActionResult_When_Successful()
        {
            var trailerId = 1;
            var brand = "UpdatedBrand";
            var model = "UpdatedModel";
            var type = "UpdatedType";
            var mileage = 1200f;
            var maxLoad = 2500f;
            var licensePlate = "XYZ987";
            var yearOfProduction = 2012;

            var result = await _controller.UpdateThisTrailer(trailerId, brand, model, type, mileage, maxLoad, licensePlate, yearOfProduction);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }
    }
}