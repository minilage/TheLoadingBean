using MongoDB.Driver;
using Moq;
using TheLoadingBean.Shared.Models;
using TheLoadingBeanAPI.Data;

namespace TheLoadingBean.Tests.Repositories
{
    public class ProductRepositoryTests
    {
        private readonly Mock<IMongoCollection<Product>> _mockCollection;
        private readonly ProductRepository _repository;

        public ProductRepositoryTests()
        {
            _mockCollection = new Mock<IMongoCollection<Product>>();
            _repository = new ProductRepository(_mockCollection.Object);
        }

        [Fact]
        public async Task GetAllProductsAsync_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new() { Id = "1", Name = "Test Product 1" },
                new() { Id = "2", Name = "Test Product 2" }
            };
            var mockCursor = new Mock<IAsyncCursor<Product>>();
            mockCursor.Setup(c => c.Current).Returns(products);
            mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            _mockCollection.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Product>>(),
                It.IsAny<FindOptions<Product>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetAllProductsAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Test Product 1", result[0].Name);
            Assert.Equal("Test Product 2", result[1].Name);
        }

        [Fact]
        public async Task GetProductByIdAsync_WithValidId_ReturnsProduct()
        {
            // Arrange
            var product = new Product { Id = "1", Name = "Test Product" };
            var mockCursor = new Mock<IAsyncCursor<Product>>();
            mockCursor.Setup(c => c.Current).Returns(new List<Product> { product });
            mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            _mockCollection.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Product>>(),
                It.IsAny<FindOptions<Product>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetProductByIdAsync("1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
            Assert.Equal("Test Product", result.Name);
        }

        [Fact]
        public async Task GetProductByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var mockCursor = new Mock<IAsyncCursor<Product>>();
            mockCursor.Setup(c => c.Current).Returns(new List<Product>());
            mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            _mockCollection.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Product>>(),
                It.IsAny<FindOptions<Product>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetProductByIdAsync("1");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateProductAsync_AddsProductToCollection()
        {
            // Arrange
            var product = new Product { Name = "New Product" };
            _mockCollection.Setup(c => c.InsertOneAsync(
                It.IsAny<Product>(),
                It.IsAny<InsertOneOptions>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _repository.CreateProductAsync(product);

            // Assert
            _mockCollection.Verify(c => c.InsertOneAsync(
                It.Is<Product>(p => p.Name == "New Product"),
                It.IsAny<InsertOneOptions>(),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateProductAsync_UpdatesProductInCollection()
        {
            // Arrange
            var product = new Product { Id = "1", Name = "Updated Product" };
            var updateResult = new UpdateResult.Acknowledged(1, 1, null);
            _mockCollection.Setup(c => c.UpdateOneAsync(
                It.IsAny<FilterDefinition<Product>>(),
                It.IsAny<UpdateDefinition<Product>>(),
                It.IsAny<UpdateOptions>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(updateResult);

            // Act
            await _repository.UpdateProductAsync("1", product);

            // Assert
            _mockCollection.Verify(c => c.UpdateOneAsync(
                It.IsAny<FilterDefinition<Product>>(),
                It.IsAny<UpdateDefinition<Product>>(),
                It.IsAny<UpdateOptions>(),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_DeletesProductFromCollection()
        {
            // Arrange
            var deleteResult = new DeleteResult.Acknowledged(1);
            _mockCollection.Setup(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<Product>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(deleteResult);

            // Act
            var result = await _repository.DeleteProductAsync("1");

            // Assert
            Assert.True(result);
            _mockCollection.Verify(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<Product>>(),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_WithInvalidId_ReturnsFalse()
        {
            // Arrange
            var deleteResult = new DeleteResult.Acknowledged(0);
            _mockCollection.Setup(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<Product>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(deleteResult);

            // Act
            var result = await _repository.DeleteProductAsync("1");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task SearchProductsAsync_ReturnsMatchingProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new() { Id = "1", Name = "Coffee Maker" },
                new() { Id = "2", Name = "Coffee Beans" }
            };
            var mockCursor = new Mock<IAsyncCursor<Product>>();
            mockCursor.Setup(c => c.Current).Returns(products);
            mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            _mockCollection.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Product>>(),
                It.IsAny<FindOptions<Product>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.SearchProductsAsync("Coffee");

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, p => Assert.Contains("Coffee", p.Name));
        }
    }
} 