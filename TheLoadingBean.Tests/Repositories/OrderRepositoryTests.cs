using MongoDB.Driver;
using Moq;
using TheLoadingBean.Shared.Models;
using TheLoadingBeanAPI.Data;

namespace TheLoadingBean.Tests.Repositories
{
    public class OrderRepositoryTests
    {
        private readonly Mock<IMongoCollection<Order>> _mockCollection;
        private readonly OrderRepository _repository;

        public OrderRepositoryTests()
        {
            _mockCollection = new Mock<IMongoCollection<Order>>();
            _repository = new OrderRepository(_mockCollection.Object);
        }

        [Fact]
        public async Task GetAllOrdersAsync_ReturnsAllOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new() { Id = "1", CustomerId = "1", TotalAmount = 10.99m },
                new() { Id = "2", CustomerId = "2", TotalAmount = 20.99m }
            };
            var mockCursor = new Mock<IAsyncCursor<Order>>();
            mockCursor.Setup(c => c.Current).Returns(orders);
            mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            _mockCollection.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Order>>(),
                It.IsAny<FindOptions<Order>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetAllOrdersAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("1", result[0].CustomerId);
            Assert.Equal("2", result[1].CustomerId);
        }

        [Fact]
        public async Task GetOrderByIdAsync_WithValidId_ReturnsOrder()
        {
            // Arrange
            var order = new Order { Id = "1", CustomerId = "1", TotalAmount = 10.99m };
            var mockCursor = new Mock<IAsyncCursor<Order>>();
            mockCursor.Setup(c => c.Current).Returns(new List<Order> { order });
            mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            _mockCollection.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Order>>(),
                It.IsAny<FindOptions<Order>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetOrderByIdAsync("1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
            Assert.Equal("1", result.CustomerId);
            Assert.Equal(10.99m, result.TotalAmount);
        }

        [Fact]
        public async Task GetOrderByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var mockCursor = new Mock<IAsyncCursor<Order>>();
            mockCursor.Setup(c => c.Current).Returns(new List<Order>());
            mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            _mockCollection.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Order>>(),
                It.IsAny<FindOptions<Order>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetOrderByIdAsync("1");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetOrdersByCustomerIdAsync_WithValidCustomerId_ReturnsOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new() { Id = "1", CustomerId = "1", TotalAmount = 10.99m },
                new() { Id = "2", CustomerId = "1", TotalAmount = 20.99m }
            };
            var mockCursor = new Mock<IAsyncCursor<Order>>();
            mockCursor.Setup(c => c.Current).Returns(orders);
            mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            _mockCollection.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Order>>(),
                It.IsAny<FindOptions<Order>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetOrdersByCustomerIdAsync("1");

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, order => Assert.Equal("1", order.CustomerId));
        }

        [Fact]
        public async Task CreateOrderAsync_AddsOrderToCollection()
        {
            // Arrange
            var order = new Order { CustomerId = "1", TotalAmount = 10.99m };
            _mockCollection.Setup(c => c.InsertOneAsync(
                It.IsAny<Order>(),
                It.IsAny<InsertOneOptions>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _repository.CreateOrderAsync(order);

            // Assert
            _mockCollection.Verify(c => c.InsertOneAsync(
                It.Is<Order>(o => o.CustomerId == "1" && o.TotalAmount == 10.99m),
                It.IsAny<InsertOneOptions>(),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateOrderAsync_UpdatesOrderInCollection()
        {
            // Arrange
            var order = new Order { Id = "1", CustomerId = "1", TotalAmount = 15.99m };
            var updateResult = new UpdateResult.Acknowledged(1, 1, null);
            _mockCollection.Setup(c => c.UpdateOneAsync(
                It.IsAny<FilterDefinition<Order>>(),
                It.IsAny<UpdateDefinition<Order>>(),
                It.IsAny<UpdateOptions>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(updateResult);

            // Act
            await _repository.UpdateOrderAsync("1", order);

            // Assert
            _mockCollection.Verify(c => c.UpdateOneAsync(
                It.IsAny<FilterDefinition<Order>>(),
                It.IsAny<UpdateDefinition<Order>>(),
                It.IsAny<UpdateOptions>(),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task DeleteOrderAsync_DeletesOrderFromCollection()
        {
            // Arrange
            var deleteResult = new DeleteResult.Acknowledged(1);
            _mockCollection.Setup(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<Order>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(deleteResult);

            // Act
            var result = await _repository.DeleteOrderAsync("1");

            // Assert
            Assert.True(result);
            _mockCollection.Verify(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<Order>>(),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task DeleteOrderAsync_WithInvalidId_ReturnsFalse()
        {
            // Arrange
            var deleteResult = new DeleteResult.Acknowledged(0);
            _mockCollection.Setup(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<Order>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(deleteResult);

            // Act
            var result = await _repository.DeleteOrderAsync("1");

            // Assert
            Assert.False(result);
        }
    }
} 