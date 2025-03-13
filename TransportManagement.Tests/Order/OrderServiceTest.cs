using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using TransportManagement;
using TransportManagement.Models.Orders;
using TransportManagement.Models.User;
using TransportManagement.Services.Order;
using Xunit;

namespace TransportManagement.Tests
{
    public class OrderServiceTests
    {
        private readonly TransportManagementDbContext _context;
        private readonly OrderService _orderService;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;

        public OrderServiceTests()
        {
            var options = new DbContextOptionsBuilder<TransportManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unikalna baza dla każdego testu
                .Options;

            _context = new TransportManagementDbContext(options);

            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            _orderService = new OrderService(_context, _userManagerMock.Object);
        }

        private OrderModel CreateValidOrder(int id)
        {
            return new OrderModel
            {
                Id = id,
                PickupLocation = "StartLocation",
                DeliveryLocation = "EndLocation",
                AssignedBy = "admin",
                DriverEmail = "driver@example.com",
                LoadType = "General Cargo",
                OrderNumber = Guid.NewGuid().ToString(),
                Status = OrderStatus.Trwa,
                Revenue = 5000,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2)
            };
        }

        [Fact]
        public async Task CreateOrderAsync_Should_Add_Order_To_Database()
        {
            var order = CreateValidOrder(1);

            await _orderService.CreateOrderAsync(
                order.StartDate,
                order.EndDate,
                order.PickupLocation,
                order.DeliveryLocation,
                order.AssignedBy,
                order.DriverEmail,
                order.LoadType,
                order.Revenue
            );

            var orders = await _context.Orders.ToListAsync();
            Assert.Single(orders);
        }

        [Fact]
        public async Task DeleteOrderAsync_Should_Remove_Order_From_Database()
        {
            var order = CreateValidOrder(2);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            await _orderService.DeleteOrderAsync(2);

            var deletedOrder = await _context.Orders.FindAsync(2);
            Assert.Null(deletedOrder);
        }

        [Fact]
        public async Task GetOrderByIdAsync_Should_Return_Order()
        {
            var order = CreateValidOrder(3);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var result = await _orderService.GetOrderByIdAsync(3);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAllOrdersAsync_Should_Return_All_Orders()
        {
            _context.Orders.AddRange(new List<OrderModel> { CreateValidOrder(4), CreateValidOrder(5) });
            await _context.SaveChangesAsync();

            var result = await _orderService.GetAllOrdersAsync();
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task UpdateOrderAsync_Should_Update_Order_Details()
        {
            var order = CreateValidOrder(6);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            bool result = await _orderService.UpdateOrderAsync(6, "admin", DateTime.Now, DateTime.Now.AddDays(5), "NewLocation", "EndLocation", "driver@example.com", "Heavy Cargo", 7000);
            var updatedOrder = await _context.Orders.FindAsync(6);

            Assert.True(result);
            Assert.Equal("NewLocation", updatedOrder.PickupLocation);
            Assert.Equal(7000, updatedOrder.Revenue);
        }

        [Fact]
        public async Task UpdateOrderStatus_Should_Change_Order_Status()
        {
            var order = CreateValidOrder(7);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            bool result = await _orderService.UpdateOrderStatus(7, OrderStatus.Zakończone);
            var updatedOrder = await _context.Orders.FindAsync(7);

            Assert.True(result);
            Assert.Equal(OrderStatus.Zakończone, updatedOrder.Status);
        }
    }
}