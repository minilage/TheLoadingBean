using Microsoft.AspNetCore.Mvc;
using Moq;
using TheLoadingBean.Shared.DTOs;
using TheLoadingBean.Shared.Models;
using TheLoadingBeanAPI.Controllers;
using TheLoadingBeanAPI.Data;

namespace TheLoadingBean.Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new ProductController(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetAllProducts_ReturnsOkResult_WithListOfProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new() { Id = "1", Name = "Test Product 1" },
                new() { Id = "2", Name = "Test Product 2" }
            };
            _mockUnitOfWork.Setup(u => u.Products.GetAllProductsAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _controller.GetAllProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProducts = Assert.IsType<List<ProductResponseDto>>(okResult.Value);
            Assert.Equal(2, returnedProducts.Count);
        }

        [Fact]
        public async Task GetProduct_WithValidId_ReturnsOkResult_WithProduct()
        {
            // Arrange
            var product = new Product { Id = "1", Name = "Test Product" };
            _mockUnitOfWork.Setup(u => u.Products.GetProductByIdAsync("1"))
                .ReturnsAsync(product);

            // Act
            var result = await _controller.GetProduct("1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProduct = Assert.IsType<ProductResponseDto>(okResult.Value);
            Assert.Equal("1", returnedProduct.Id);
            Assert.Equal("Test Product", returnedProduct.Name);
        }

        [Fact]
        public async Task GetProduct_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Products.GetProductByIdAsync("1"))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _controller.GetProduct("1");

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateProduct_WithValidData_ReturnsCreatedResult()
        {
            // Arrange
            var createProductDto = new CreateProductDto
            {
                Name = "New Product",
                Price = 10.99m,
                Category = "Test Category"
            };
            var product = new Product
            {
                Id = "1",
                Name = createProductDto.Name,
                Price = createProductDto.Price,
                Category = createProductDto.Category
            };
            _mockUnitOfWork.Setup(u => u.Products.CreateProductAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateProduct(createProductDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedProduct = Assert.IsType<ProductResponseDto>(createdResult.Value);
            Assert.Equal(createProductDto.Name, returnedProduct.Name);
            Assert.Equal(createProductDto.Price, returnedProduct.Price);
            Assert.Equal(createProductDto.Category, returnedProduct.Category);
        }

        [Fact]
        public async Task UpdateProduct_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var updateProductDto = new UpdateProductDto
            {
                Name = "Updated Product",
                Price = 15.99m,
                Category = "Updated Category"
            };
            var existingProduct = new Product
            {
                Id = "1",
                Name = "Original Product",
                Price = 10.99m,
                Category = "Original Category"
            };
            _mockUnitOfWork.Setup(u => u.Products.GetProductByIdAsync("1"))
                .ReturnsAsync(existingProduct);
            _mockUnitOfWork.Setup(u => u.Products.UpdateProductAsync("1", It.IsAny<Product>()))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateProduct("1", updateProductDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProduct = Assert.IsType<ProductResponseDto>(okResult.Value);
            Assert.Equal(updateProductDto.Name, returnedProduct.Name);
            Assert.Equal(updateProductDto.Price, returnedProduct.Price);
            Assert.Equal(updateProductDto.Category, returnedProduct.Category);
        }

        [Fact]
        public async Task UpdateProduct_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var updateProductDto = new UpdateProductDto
            {
                Name = "Updated Product"
            };
            _mockUnitOfWork.Setup(u => u.Products.GetProductByIdAsync("1"))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _controller.UpdateProduct("1", updateProductDto);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task DeleteProduct_WithValidId_ReturnsNoContent()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Products.DeleteProductAsync("1"))
                .ReturnsAsync(true);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteProduct("1");

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Products.DeleteProductAsync("1"))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteProduct("1");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
} 