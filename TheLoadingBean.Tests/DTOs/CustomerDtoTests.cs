using System.ComponentModel.DataAnnotations;
using TheLoadingBean.Shared.DTOs;

namespace TheLoadingBean.Tests.DTOs
{
    public class CustomerDtoTests
    {
        [Fact]
        public void CreateCustomerDto_WithValidData_IsValid()
        {
            // Arrange
            var dto = new CreateCustomerDto
            {
                Name = "Test Customer",
                Email = "test@example.com",
                Phone = "1234567890",
                Password = "Password123!"
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
        public void CreateCustomerDto_WithInvalidEmail_IsInvalid()
        {
            // Arrange
            var dto = new CreateCustomerDto
            {
                Name = "Test Customer",
                Email = "invalid-email",
                Password = "Password123!"
            };

            // Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Contains("Invalid email format", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void CreateCustomerDto_WithInvalidPassword_IsInvalid()
        {
            // Arrange
            var dto = new CreateCustomerDto
            {
                Name = "Test Customer",
                Email = "test@example.com",
                Password = "short"
            };

            // Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Contains("Password must be at least 8 characters long", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void UpdateCustomerDto_WithValidData_IsValid()
        {
            // Arrange
            var dto = new UpdateCustomerDto
            {
                Name = "Updated Customer",
                Email = "updated@example.com",
                Phone = "0987654321"
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
        public void UpdateCustomerDto_WithInvalidEmail_IsInvalid()
        {
            // Arrange
            var dto = new UpdateCustomerDto
            {
                Name = "Updated Customer",
                Email = "invalid-email"
            };

            // Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Contains("Invalid email format", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void CustomerResponseDto_WithValidData_IsValid()
        {
            // Arrange
            var dto = new CustomerResponseDto
            {
                Id = "1",
                Name = "Test Customer",
                Email = "test@example.com",
                Phone = "1234567890",
                IsAdmin = false
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
        public void CustomerResponseDto_WithInvalidEmail_IsInvalid()
        {
            // Arrange
            var dto = new CustomerResponseDto
            {
                Id = "1",
                Name = "Test Customer",
                Email = "invalid-email"
            };

            // Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Contains("Invalid email format", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void LoginDto_WithValidData_IsValid()
        {
            // Arrange
            var dto = new LoginDto
            {
                Email = "test@example.com",
                Password = "Password123!"
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
        public void LoginDto_WithInvalidEmail_IsInvalid()
        {
            // Arrange
            var dto = new LoginDto
            {
                Email = "invalid-email",
                Password = "Password123!"
            };

            // Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Contains("Invalid email format", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void RegisterDto_WithValidData_IsValid()
        {
            // Arrange
            var dto = new RegisterDto
            {
                Name = "Test Customer",
                Email = "test@example.com",
                Phone = "1234567890",
                Password = "Password123!"
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
        public void RegisterDto_WithInvalidEmail_IsInvalid()
        {
            // Arrange
            var dto = new RegisterDto
            {
                Name = "Test Customer",
                Email = "invalid-email",
                Password = "Password123!"
            };

            // Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Contains("Invalid email format", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void RegisterDto_WithInvalidPassword_IsInvalid()
        {
            // Arrange
            var dto = new RegisterDto
            {
                Name = "Test Customer",
                Email = "test@example.com",
                Password = "short"
            };

            // Act
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Contains("Password must be at least 8 characters long", validationResults[0].ErrorMessage);
        }
    }
} 