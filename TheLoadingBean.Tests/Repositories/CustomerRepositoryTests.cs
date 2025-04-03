using MongoDB.Driver;
using Moq;
using TheLoadingBean.Shared.Models;
using TheLoadingBeanAPI.Data;

namespace TheLoadingBean.Tests.Repositories
{
    public class CustomerRepositoryTests
    {
        private readonly Mock<IMongoCollection<Customer>> _mockCollection;
        private readonly CustomerRepository _repository;

        public CustomerRepositoryTests()
        {
            _mockCollection = new Mock<IMongoCollection<Customer>>();
            _repository = new CustomerRepository(_mockCollection.Object);
        }

        [Fact]
        public async Task GetAllCustomersAsync_ReturnsAllCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new() { Id = "1", Name = "Test Customer 1" },
                new() { Id = "2", Name = "Test Customer 2" }
            };
            var mockCursor = new Mock<IAsyncCursor<Customer>>();
            mockCursor.Setup(c => c.Current).Returns(customers);
            mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            _mockCollection.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Customer>>(),
                It.IsAny<FindOptions<Customer>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetAllCustomersAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Test Customer 1", result[0].Name);
            Assert.Equal("Test Customer 2", result[1].Name);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_WithValidId_ReturnsCustomer()
        {
            // Arrange
            var customer = new Customer { Id = "1", Name = "Test Customer" };
            var mockCursor = new Mock<IAsyncCursor<Customer>>();
            mockCursor.Setup(c => c.Current).Returns(new List<Customer> { customer });
            mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            _mockCollection.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Customer>>(),
                It.IsAny<FindOptions<Customer>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetCustomerByIdAsync("1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
            Assert.Equal("Test Customer", result.Name);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var mockCursor = new Mock<IAsyncCursor<Customer>>();
            mockCursor.Setup(c => c.Current).Returns(new List<Customer>());
            mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            _mockCollection.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Customer>>(),
                It.IsAny<FindOptions<Customer>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetCustomerByIdAsync("1");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetCustomerByEmailAsync_WithValidEmail_ReturnsCustomer()
        {
            // Arrange
            var customer = new Customer { Id = "1", Email = "test@example.com" };
            var mockCursor = new Mock<IAsyncCursor<Customer>>();
            mockCursor.Setup(c => c.Current).Returns(new List<Customer> { customer });
            mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            _mockCollection.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Customer>>(),
                It.IsAny<FindOptions<Customer>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetCustomerByEmailAsync("test@example.com");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
            Assert.Equal("test@example.com", result.Email);
        }

        [Fact]
        public async Task GetCustomerByEmailAsync_WithInvalidEmail_ReturnsNull()
        {
            // Arrange
            var mockCursor = new Mock<IAsyncCursor<Customer>>();
            mockCursor.Setup(c => c.Current).Returns(new List<Customer>());
            mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            _mockCollection.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Customer>>(),
                It.IsAny<FindOptions<Customer>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetCustomerByEmailAsync("nonexistent@example.com");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateCustomerAsync_AddsCustomerToCollection()
        {
            // Arrange
            var customer = new Customer { Name = "New Customer" };
            _mockCollection.Setup(c => c.InsertOneAsync(
                It.IsAny<Customer>(),
                It.IsAny<InsertOneOptions>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _repository.CreateCustomerAsync(customer);

            // Assert
            _mockCollection.Verify(c => c.InsertOneAsync(
                It.Is<Customer>(c => c.Name == "New Customer"),
                It.IsAny<InsertOneOptions>(),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateCustomerAsync_UpdatesCustomerInCollection()
        {
            // Arrange
            var customer = new Customer { Id = "1", Name = "Updated Customer" };
            var updateResult = new UpdateResult.Acknowledged(1, 1, null);
            _mockCollection.Setup(c => c.UpdateOneAsync(
                It.IsAny<FilterDefinition<Customer>>(),
                It.IsAny<UpdateDefinition<Customer>>(),
                It.IsAny<UpdateOptions>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(updateResult);

            // Act
            await _repository.UpdateCustomerAsync("1", customer);

            // Assert
            _mockCollection.Verify(c => c.UpdateOneAsync(
                It.IsAny<FilterDefinition<Customer>>(),
                It.IsAny<UpdateDefinition<Customer>>(),
                It.IsAny<UpdateOptions>(),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task DeleteCustomerAsync_DeletesCustomerFromCollection()
        {
            // Arrange
            var deleteResult = new DeleteResult.Acknowledged(1);
            _mockCollection.Setup(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<Customer>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(deleteResult);

            // Act
            var result = await _repository.DeleteCustomerAsync("1");

            // Assert
            Assert.True(result);
            _mockCollection.Verify(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<Customer>>(),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task DeleteCustomerAsync_WithInvalidId_ReturnsFalse()
        {
            // Arrange
            var deleteResult = new DeleteResult.Acknowledged(0);
            _mockCollection.Setup(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<Customer>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(deleteResult);

            // Act
            var result = await _repository.DeleteCustomerAsync("1");

            // Assert
            Assert.False(result);
        }
    }
} 