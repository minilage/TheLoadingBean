using System.ComponentModel.DataAnnotations;
using TheLoadingBean.Shared.DTOs;

namespace TheLoadingBean.Tests.DTOs
{
    public class OrderDtoTests
    {
        [Fact]
        public void CreateOrderDto_WithValidData_IsValid()
        {
            // Arrange
            var dto = new CreateOrderDto
            {
                CustomerId = "1",
                Items = new List<OrderItemDto>
                {
                    new() { ProductId = "1", Quantity = 2 },
                    new() { ProductId = "2", Quantity = 1 }
                }
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
        public void CreateOrderDto_WithEmptyItems_IsInvalid()
        {
            // Arrange
            var dto = new CreateOrderDto
            {
                CustomerId = "1",
                Items = new List<OrderItemDto>()
            };

            // Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Contains("At least one item is required", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void UpdateOrderDto_WithValidData_IsValid()
        {
            // Arrange
            var dto = new UpdateOrderDto
            {
                Items = new List<OrderItemDto>
                {
                    new() { ProductId = "1", Quantity = 3 },
                    new() { ProductId = "2", Quantity = 2 }
                }
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
        public void UpdateOrderDto_WithEmptyItems_IsInvalid()
        {
            // Arrange
            var dto = new UpdateOrderDto
            {
                Items = new List<OrderItemDto>()
            };

            // Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Contains("At least one item is required", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void OrderResponseDto_WithValidData_IsValid()
        {
            // Arrange
            var dto = new OrderResponseDto
            {
                Id = "1",
                CustomerId = "1",
                Items = new List<OrderItemDto>
                {
                    new() { ProductId = "1", Quantity = 2 },
                    new() { ProductId = "2", Quantity = 1 }
                },
                TotalAmount = 29.97m,
                OrderDate = DateTime.UtcNow
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
        public void OrderResponseDto_WithEmptyItems_IsInvalid()
        {
            // Arrange
            var dto = new OrderResponseDto
            {
                Id = "1",
                CustomerId = "1",
                Items = new List<OrderItemDto>(),
                TotalAmount = 0m,
                OrderDate = DateTime.UtcNow
            };

            // Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Contains("At least one item is required", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void OrderResponseDto_WithNegativeTotalAmount_IsInvalid()
        {
            // Arrange
            var dto = new OrderResponseDto
            {
                Id = "1",
                CustomerId = "1",
                Items = new List<OrderItemDto>
                {
                    new() { ProductId = "1", Quantity = 1 }
                },
                TotalAmount = -10.99m,
                OrderDate = DateTime.UtcNow
            };

            // Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Contains("Total amount must be greater than 0", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void OrderItemDto_WithValidData_IsValid()
        {
            // Arrange
            var dto = new OrderItemDto
            {
                ProductId = "1",
                Quantity = 2
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
        public void OrderItemDto_WithInvalidQuantity_IsInvalid()
        {
            // Arrange
            var dto = new OrderItemDto
            {
                ProductId = "1",
                Quantity = 0
            };

            // Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Contains("Quantity must be greater than 0", validationResults[0].ErrorMessage);
        }
    }
} 