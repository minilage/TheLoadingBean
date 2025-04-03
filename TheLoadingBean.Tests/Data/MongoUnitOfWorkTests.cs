using MongoDB.Driver;
using Moq;
using TheLoadingBean.Shared.Models;
using TheLoadingBeanAPI.Data;

namespace TheLoadingBean.Tests.Data
{
    public class MongoUnitOfWorkTests
    {
        private readonly Mock<IMongoDatabase> _mockDatabase;
        private readonly MongoUnitOfWork _unitOfWork;

        public MongoUnitOfWorkTests()
        {
            _mockDatabase = new Mock<IMongoDatabase>();
            _unitOfWork = new MongoUnitOfWork(_mockDatabase.Object);
        }

        [Fact]
        public void Products_ReturnsProductRepository()
        {
            // Act
            var result = _unitOfWork.Products;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProductRepository>(result);
        }

        [Fact]
        public void Customers_ReturnsCustomerRepository()
        {
            // Act
            var result = _unitOfWork.Customers;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CustomerRepository>(result);
        }

        [Fact]
        public void Orders_ReturnsOrderRepository()
        {
            // Act
            var result = _unitOfWork.Orders;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OrderRepository>(result);
        }

        [Fact]
        public async Task SaveChangesAsync_CommitsTransaction()
        {
            // Arrange
            var mockSession = new Mock<IClientSessionHandle>();
            _mockDatabase.Setup(d => d.Client.StartSession())
                .Returns(mockSession.Object);

            // Act
            await _unitOfWork.SaveChangesAsync();

            // Assert
            mockSession.Verify(s => s.StartTransaction(), Times.Once);
            mockSession.Verify(s => s.CommitTransaction(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SaveChangesAsync_WhenErrorOccurs_RollsBackTransaction()
        {
            // Arrange
            var mockSession = new Mock<IClientSessionHandle>();
            _mockDatabase.Setup(d => d.Client.StartSession())
                .Returns(mockSession.Object);
            mockSession.Setup(s => s.CommitTransaction(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Test error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _unitOfWork.SaveChangesAsync());
            mockSession.Verify(s => s.StartTransaction(), Times.Once);
            mockSession.Verify(s => s.CommitTransaction(It.IsAny<CancellationToken>()), Times.Once);
            mockSession.Verify(s => s.AbortTransaction(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Dispose_DisposesSession()
        {
            // Arrange
            var mockSession = new Mock<IClientSessionHandle>();
            _mockDatabase.Setup(d => d.Client.StartSession())
                .Returns(mockSession.Object);

            // Act
            _unitOfWork.Dispose();

            // Assert
            mockSession.Verify(s => s.Dispose(), Times.Once);
        }

        [Fact]
        public async Task BeginTransactionAsync_StartsNewTransaction()
        {
            // Arrange
            var mockSession = new Mock<IClientSessionHandle>();
            _mockDatabase.Setup(d => d.Client.StartSession())
                .Returns(mockSession.Object);

            // Act
            await _unitOfWork.BeginTransactionAsync();

            // Assert
            mockSession.Verify(s => s.StartTransaction(), Times.Once);
        }

        [Fact]
        public async Task CommitTransactionAsync_CommitsTransaction()
        {
            // Arrange
            var mockSession = new Mock<IClientSessionHandle>();
            _mockDatabase.Setup(d => d.Client.StartSession())
                .Returns(mockSession.Object);

            // Act
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.CommitTransactionAsync();

            // Assert
            mockSession.Verify(s => s.CommitTransaction(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RollbackTransactionAsync_RollsBackTransaction()
        {
            // Arrange
            var mockSession = new Mock<IClientSessionHandle>();
            _mockDatabase.Setup(d => d.Client.StartSession())
                .Returns(mockSession.Object);

            // Act
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.RollbackTransactionAsync();

            // Assert
            mockSession.Verify(s => s.AbortTransaction(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
} 