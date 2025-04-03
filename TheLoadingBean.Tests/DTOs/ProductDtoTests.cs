using System.ComponentModel.DataAnnotations;
using TheLoadingBean.Shared.DTOs;

namespace TheLoadingBean.Tests.DTOs
{
    public class ProductDtoTests
    {
        [Fact]
        public void CreateProductDto_WithValidData_IsValid()
        {
            // Arrange
            var dto = new CreateProductDto
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 10.99m,
                Category = "Test Category",
                StockQuantity = 100,
                IsAvailable = true
            };

            // Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }

        [Fact]
        public void CreateProductDto_WithInvalidPrice_IsInvalid()
        {
            // Arrange
            var dto = new CreateProductDto
            {
                Name = "Test Product",
                Price = -10.99m
            };

            // Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Contains("Price must be greater than 0", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void CreateProductDto_WithInvalidStockQuantity_IsInvalid()
        {
            // Arrange
            var dto = new CreateProductDto
            {
                Name = "Test Product",
                StockQuantity = -1
            };

            // Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Contains("Stock quantity must be greater than or equal to 0", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void UpdateProductDto_WithValidData_IsValid()
        {
            // Arrange
            var dto = new UpdateProductDto
            {
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 15.99m,
                Category = "Updated Category",
                StockQuantity = 50,
                IsAvailable = true
            };

            // Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }

        [Fact]
        public void UpdateProductDto_WithInvalidPrice_IsInvalid()
        {
            // Arrange
            var dto = new UpdateProductDto
            {
                Name = "Updated Product",
                Price = -15.99m
            };

            // Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Contains("Price must be greater than 0", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void UpdateProductDto_WithInvalidStockQuantity_IsInvalid()
        {
            // Arrange
            var dto = new UpdateProductDto
            {
                Name = "Updated Product",
                StockQuantity = -1
            };

            // Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Contains("Stock quantity must be greater than or equal to 0", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void ProductResponseDto_WithValidData_IsValid()
        {
            // Arrange
            var dto = new ProductResponseDto
            {
                Id = "1",
                Name = "Test Product",
                Description = "Test Description",
                Price = 10.99m,
                Category = "Test Category",
                StockQuantity = 100,
                IsAvailable = true
            };

            // Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }

        [Fact]
        public void ProductResponseDto_WithInvalidPrice_IsInvalid()
        {
            // Arrange
            var dto = new ProductResponseDto
            {
                Id = "1",
                Name = "Test Product",
                Price = -10.99m
            };

            // Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Contains("Price must be greater than 0", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void ProductResponseDto_WithInvalidStockQuantity_IsInvalid()
        {
            // Arrange
            var dto = new ProductResponseDto
            {
                Id = "1",
                Name = "Test Product",
                StockQuantity = -1
            };

            // Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Contains("Stock quantity must be greater than or equal to 0", validationResults[0].ErrorMessage);
        }
    }
} 