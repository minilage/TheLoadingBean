using Microsoft.AspNetCore.Mvc;
using Moq;
using TheLoadingBean.Shared.DTOs;
using TheLoadingBean.Shared.Models;
using TheLoadingBeanAPI.Controllers;
using TheLoadingBeanAPI.Data;

namespace TheLoadingBean.Tests.Controllers
{
    public class OrderControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new OrderController(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetAllOrders_ReturnsOkResult_WithListOfOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new() { Id = "1", CustomerId = "1", TotalAmount = 10.99m },
                new() { Id = "2", CustomerId = "2", TotalAmount = 20.99m }
            };
            _mockUnitOfWork.Setup(u => u.Orders.GetAllOrdersAsync())
                .ReturnsAsync(orders);

            // Act
            var result = await _controller.GetAllOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedOrders = Assert.IsType<List<OrderResponseDto>>(okResult.Value);
            Assert.Equal(2, returnedOrders.Count);
        }

        [Fact]
        public async Task GetOrder_WithValidId_ReturnsOkResult_WithOrder()
        {
            // Arrange
            var order = new Order { Id = "1", CustomerId = "1", TotalAmount = 10.99m };
            _mockUnitOfWork.Setup(u => u.Orders.GetOrderByIdAsync("1"))
                .ReturnsAsync(order);

            // Act
            var result = await _controller.GetOrder("1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedOrder = Assert.IsType<OrderResponseDto>(okResult.Value);
            Assert.Equal("1", returnedOrder.Id);
            Assert.Equal("1", returnedOrder.CustomerId);
            Assert.Equal(10.99m, returnedOrder.TotalAmount);
        }

        [Fact]
        public async Task GetOrder_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Orders.GetOrderByIdAsync("1"))
                .ReturnsAsync((Order)null);

            // Act
            var result = await _controller.GetOrder("1");

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateOrder_WithValidData_ReturnsCreatedResult()
        {
            // Arrange
            var createOrderDto = new CreateOrderDto
            {
                CustomerId = "customer1",
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto
                    {
                        ProductId = "product1",
                        Quantity = 2,
                        UnitPrice = 10.0m
                    }
                }
            };

            var order = new Order
            {
                Id = "order1",
                CustomerId = "customer1",
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto
                    {
                        ProductId = "product1",
                        Quantity = 2,
                        UnitPrice = 10.0m
                    }
                },
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                TotalAmount = 20.0m
            };
            _mockUnitOfWork.Setup(u => u.Orders.CreateOrderAsync(It.IsAny<Order>()))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateOrder(createOrderDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedOrder = Assert.IsType<OrderResponseDto>(createdResult.Value);
            Assert.Equal(createOrderDto.CustomerId, returnedOrder.CustomerId);
            Assert.Single(returnedOrder.Items);
            Assert.Equal(2, returnedOrder.Items[0].Quantity);
        }

        [Fact]
        public async Task UpdateOrder_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var updateOrderDto = new UpdateOrderDto
            {
                Items = new List<OrderItemDto>
                {
                    new() { ProductId = "1", Quantity = 3 }
                }
            };
            var existingOrder = new Order
            {
                Id = "1",
                CustomerId = "1",
                Items = new List<OrderItem>
                {
                    new() { ProductId = "1", Quantity = 2 }
                },
                TotalAmount = 21.98m
            };
            _mockUnitOfWork.Setup(u => u.Orders.GetOrderByIdAsync("1"))
                .ReturnsAsync(existingOrder);
            _mockUnitOfWork.Setup(u => u.Orders.UpdateOrderAsync("1", It.IsAny<Order>()))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateOrder("1", updateOrderDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedOrder = Assert.IsType<OrderResponseDto>(okResult.Value);
            Assert.Single(returnedOrder.Items);
            Assert.Equal(3, returnedOrder.Items[0].Quantity);
        }

        [Fact]
        public async Task UpdateOrder_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var updateOrderDto = new UpdateOrderDto
            {
                Items = new List<OrderItemDto>
                {
                    new() { ProductId = "1", Quantity = 3 }
                }
            };
            _mockUnitOfWork.Setup(u => u.Orders.GetOrderByIdAsync("1"))
                .ReturnsAsync((Order)null);

            // Act
            var result = await _controller.UpdateOrder("1", updateOrderDto);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task DeleteOrder_WithValidId_ReturnsNoContent()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Orders.DeleteOrderAsync("1"))
                .ReturnsAsync(true);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteOrder("1");

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteOrder_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Orders.DeleteOrderAsync("1"))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteOrder("1");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetCustomerOrders_WithValidCustomerId_ReturnsOkResult_WithOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new() { Id = "1", CustomerId = "1", TotalAmount = 10.99m },
                new() { Id = "2", CustomerId = "1", TotalAmount = 20.99m }
            };
            _mockUnitOfWork.Setup(u => u.Orders.GetOrdersByCustomerIdAsync("1"))
                .ReturnsAsync(orders);

            // Act
            var result = await _controller.GetCustomerOrders("1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedOrders = Assert.IsType<List<OrderResponseDto>>(okResult.Value);
            Assert.Equal(2, returnedOrders.Count);
            Assert.All(returnedOrders, order => Assert.Equal("1", order.CustomerId));
        }
    }
} 