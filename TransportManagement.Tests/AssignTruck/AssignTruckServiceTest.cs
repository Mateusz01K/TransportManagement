using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TransportManagement.Models.AssignTruck;
using TransportManagement.Models.Drivers;
using TransportManagement.Models.Truck;
using TransportManagement.Services.AssignTruck;
using Xunit;

namespace TransportManagement.Tests
{
    public class AssignTruckServiceTests
    {
        private readonly TransportManagementDbContext _context;
        private readonly AssignTruckService _assignTruckService;

        public AssignTruckServiceTests()
        {
            var options = new DbContextOptionsBuilder<TransportManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "TestAssignTruckDb")
                .EnableSensitiveDataLogging()
                .Options;

            _context = new TransportManagementDbContext(options);
            _assignTruckService = new AssignTruckService(_context);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task AssignmentTruck_Should_Assign_Truck_To_Driver_When_Valid()
        {
            var truck = new TruckModel
            {
                Id = new Random().Next(1, 1000),
                Brand = "Brand",
                LicensePlate = "XYZ123",
                Model = "Model A",
                IsAssignedDriver = false
            };

            var driver = new DriverModel
            {
                Id = new Random().Next(1, 1000),
                Name = "John",
                LastName = "Doe",
                Address = "123 Main St",
                Email = "john.doe@example.com",
                PhoneNumber = "123-456-7890",
                IsAssignedTruck = false
            };

            _context.Trucks.Add(truck);
            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();

            await _assignTruckService.AssignmentTruck(truck.Id, driver.Id);

            var assignedTruck = await _context.Trucks.FirstOrDefaultAsync(t => t.Id == truck.Id);
            var assignedDriver = await _context.Drivers.FirstOrDefaultAsync(d => d.Id == driver.Id);
            var assignment = await _context.AssignTrucks.FirstOrDefaultAsync(a => a.TruckId == truck.Id && a.DriverId == driver.Id);

            Assert.NotNull(assignedTruck);
            Assert.NotNull(assignedDriver);
            Assert.True(assignedTruck.IsAssignedDriver);
            Assert.True(assignedDriver.IsAssignedTruck);
            Assert.NotNull(assignment);
        }

        [Fact]
        public async Task AssignmentTruck_Should_Throw_Exception_When_Truck_Or_Driver_Not_Found()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _assignTruckService.AssignmentTruck(999, 1));
            await Assert.ThrowsAsync<ArgumentException>(() => _assignTruckService.AssignmentTruck(1, 999));
        }

        [Fact]
        public async Task AssignmentTruck_Should_Throw_Exception_When_Truck_Or_Driver_Already_Assigned()
        {
            var truck = new TruckModel
            {
                Id = 1,
                Brand = "Brand",
                LicensePlate = "XYZ123",
                Model = "Model A",
                IsAssignedDriver = true
            };

            var driver = new DriverModel
            {
                Id = 1,
                Name = "John",
                LastName = "Doe",
                Address = "123 Main St",
                Email = "john.doe@example.com",
                PhoneNumber = "123-456-7890",
                IsAssignedTruck = true
            };

            _context.Trucks.Add(truck);
            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();

            await Assert.ThrowsAsync<ArgumentException>(() => _assignTruckService.AssignmentTruck(truck.Id, driver.Id));
        }

        [Fact]
        public async Task DeleteAssignment_Should_Remove_Assignment_When_Valid()
        {
            var truck = new TruckModel
            {
                Id = new Random().Next(1, 1000),
                Brand = "Brand",
                LicensePlate = "XYZ123",
                Model = "Model A",
                IsAssignedDriver = false
            };

            var driver = new DriverModel
            {
                Id = new Random().Next(1, 1000),
                Name = "John",
                LastName = "Doe",
                Address = "123 Main St",
                Email = "john.doe@example.com",
                PhoneNumber = "123-456-7890",
                IsAssignedTruck = false
            };

            _context.Trucks.Add(truck);
            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();

            await _assignTruckService.AssignmentTruck(truck.Id, driver.Id);
            var assignment = await _context.AssignTrucks.FirstOrDefaultAsync(a => a.TruckId == truck.Id && a.DriverId == driver.Id);
            await _assignTruckService.DeleteAssignment(assignment.Id);

            var deletedAssignment = await _context.AssignTrucks.FirstOrDefaultAsync(a => a.Id == assignment.Id);
            var updatedTruck = await _context.Trucks.FirstOrDefaultAsync(t => t.Id == truck.Id);
            var updatedDriver = await _context.Drivers.FirstOrDefaultAsync(d => d.Id == driver.Id);

            Assert.Null(deletedAssignment);
            Assert.False(updatedTruck.IsAssignedDriver);
            Assert.False(updatedDriver.IsAssignedTruck);
        }

        [Fact]
        public async Task DeleteAssignment_Should_Throw_Exception_When_Assignment_Not_Found()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _assignTruckService.DeleteAssignment(999));
        }

        [Fact]
        public async Task ReturnTruck_Should_Throw_Exception_When_Assignment_Not_Found()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _assignTruckService.ReturnTruck(999));
        }

        [Fact]
        public async Task ReturnTruck_Should_Throw_Exception_When_Already_Returned()
        {
            var truck = new TruckModel
            {
                Id = new Random().Next(1, 1000),
                Brand = "Brand",
                LicensePlate = "XYZ123",
                Model = "Model A",
                IsAssignedDriver = false
            };

            var driver = new DriverModel
            {
                Id = new Random().Next(1, 1000),
                Name = "John",
                LastName = "Doe",
                Address = "123 Main St",
                Email = "john.doe@example.com",
                PhoneNumber = "123-456-7890",
                IsAssignedTruck = false
            };

            _context.Trucks.Add(truck);
            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();

            await _assignTruckService.AssignmentTruck(truck.Id, driver.Id);
            var assignment = await _context.AssignTrucks.FirstOrDefaultAsync(a => a.TruckId == truck.Id && a.DriverId == driver.Id);
            await _assignTruckService.ReturnTruck(assignment.Id);

            await Assert.ThrowsAsync<ArgumentException>(() => _assignTruckService.ReturnTruck(assignment.Id));
        }
    }
}