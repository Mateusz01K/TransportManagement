using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TransportManagement.Controllers;
using TransportManagement.Models.Orders;
using TransportManagement.Models.User;
using TransportManagement.Services.Order;
using Xunit;

namespace TransportManagement.Tests
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _orderServiceMock = new Mock<IOrderService>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            var loggerMock = new Mock<ILogger<OrderController>>();

            _controller = new OrderController(loggerMock.Object, _orderServiceMock.Object, null, _userManagerMock.Object);
        }

        [Fact]
        public async Task Create_Get_Should_Return_View()
        {
            _orderServiceMock.Setup(s => s.GetAllOrdersAsync()).ReturnsAsync(new List<OrderModel>());

            var result = await _controller.Create();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
        }

        
        [Fact]
        public async Task Edit_Get_Should_Return_View_If_Orders_Exist()
        {
            _orderServiceMock.Setup(s => s.GetAllOrdersAsync()).ReturnsAsync(new List<OrderModel> { new OrderModel() });

            var result = await _controller.Edit();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
        }

        [Fact]
        public async Task Edit_Post_Should_Redirect_To_Index_On_Success()
        {
            _orderServiceMock.Setup(s => s.UpdateOrderAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()))
                .ReturnsAsync(true);

            var result = await _controller.Edit(1, "Order123", DateTime.Now, DateTime.Now.AddDays(1), "Start", "End", "driver@example.com", "Cargo", 5000);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task UpdateStatus_Should_Redirect_To_Index_On_Success()
        {
            _orderServiceMock.Setup(s => s.UpdateOrderStatus(It.IsAny<int>(), It.IsAny<OrderStatus>()))
                .ReturnsAsync(true);

            var result = await _controller.UpdateStatus(1, OrderStatus.Zakończone);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task MarkDelivered_Should_Redirect_To_Index_On_Success()
        {
            _orderServiceMock.Setup(s => s.UpdateOrderStatus(It.IsAny<int>(), OrderStatus.Dostarczone))
                .ReturnsAsync(true);

            var result = await _controller.MarkDelivered(1);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task ArchivedOrders_Should_Return_View_With_Orders()
        {
            _orderServiceMock.Setup(s => s.GetArchivedOrders()).ReturnsAsync(new List<OrderModel>());

            var result = await _controller.ArchivedOrders();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
        }

        [Fact]
        public async Task ArchivedOrdersForDrivers_Should_Return_View_With_Orders()
        {
            var userEmail = "driver@example.com";
            _orderServiceMock.Setup(s => s.GetArchivedOrdersForDrivers(userEmail))
                .ReturnsAsync(new List<OrderModel>());

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, userEmail) };
            var identity = new ClaimsIdentity(claims, "mock");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var result = await _controller.ArchivedOrdersForDrivers();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
        }
    }
}