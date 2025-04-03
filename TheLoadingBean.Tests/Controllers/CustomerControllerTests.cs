using Microsoft.AspNetCore.Mvc;
using Moq;
using TheLoadingBean.Shared.DTOs;
using TheLoadingBean.Shared.Models;
using TheLoadingBeanAPI.Controllers;
using TheLoadingBeanAPI.Data;

namespace TheLoadingBean.Tests.Controllers
{
    public class CustomerControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly CustomerController _controller;

        public CustomerControllerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new CustomerController(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetAllCustomers_ReturnsOkResult_WithListOfCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new() { Id = "1", Name = "Test Customer 1" },
                new() { Id = "2", Name = "Test Customer 2" }
            };
            _mockUnitOfWork.Setup(u => u.Customers.GetAllCustomersAsync())
                .ReturnsAsync(customers);

            // Act
            var result = await _controller.GetAllCustomers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCustomers = Assert.IsType<List<CustomerResponseDto>>(okResult.Value);
            Assert.Equal(2, returnedCustomers.Count);
        }

        [Fact]
        public async Task GetCustomer_WithValidId_ReturnsOkResult_WithCustomer()
        {
            // Arrange
            var customer = new Customer { Id = "1", Name = "Test Customer" };
            _mockUnitOfWork.Setup(u => u.Customers.GetCustomerByIdAsync("1"))
                .ReturnsAsync(customer);

            // Act
            var result = await _controller.GetCustomer("1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCustomer = Assert.IsType<CustomerResponseDto>(okResult.Value);
            Assert.Equal("1", returnedCustomer.Id);
            Assert.Equal("Test Customer", returnedCustomer.Name);
        }

        [Fact]
        public async Task GetCustomer_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Customers.GetCustomerByIdAsync("1"))
                .ReturnsAsync((Customer)null);

            // Act
            var result = await _controller.GetCustomer("1");

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateCustomer_WithValidData_ReturnsCreatedResult()
        {
            // Arrange
            var createCustomerDto = new CreateCustomerDto
            {
                Name = "New Customer",
                Email = "test@example.com",
                Phone = "1234567890"
            };
            var customer = new Customer
            {
                Id = "1",
                Name = createCustomerDto.Name,
                Email = createCustomerDto.Email,
                Phone = createCustomerDto.Phone
            };
            _mockUnitOfWork.Setup(u => u.Customers.CreateCustomerAsync(It.IsAny<Customer>()))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateCustomer(createCustomerDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedCustomer = Assert.IsType<CustomerResponseDto>(createdResult.Value);
            Assert.Equal(createCustomerDto.Name, returnedCustomer.Name);
            Assert.Equal(createCustomerDto.Email, returnedCustomer.Email);
            Assert.Equal(createCustomerDto.Phone, returnedCustomer.Phone);
        }

        [Fact]
        public async Task UpdateCustomer_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var updateCustomerDto = new UpdateCustomerDto
            {
                Name = "Updated Customer",
                Email = "updated@example.com",
                Phone = "0987654321"
            };
            var existingCustomer = new Customer
            {
                Id = "1",
                Name = "Original Customer",
                Email = "original@example.com",
                Phone = "1234567890"
            };
            _mockUnitOfWork.Setup(u => u.Customers.GetCustomerByIdAsync("1"))
                .ReturnsAsync(existingCustomer);
            _mockUnitOfWork.Setup(u => u.Customers.UpdateCustomerAsync("1", It.IsAny<Customer>()))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateCustomer("1", updateCustomerDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCustomer = Assert.IsType<CustomerResponseDto>(okResult.Value);
            Assert.Equal(updateCustomerDto.Name, returnedCustomer.Name);
            Assert.Equal(updateCustomerDto.Email, returnedCustomer.Email);
            Assert.Equal(updateCustomerDto.Phone, returnedCustomer.Phone);
        }

        [Fact]
        public async Task UpdateCustomer_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var updateCustomerDto = new UpdateCustomerDto
            {
                Name = "Updated Customer"
            };
            _mockUnitOfWork.Setup(u => u.Customers.GetCustomerByIdAsync("1"))
                .ReturnsAsync((Customer)null);

            // Act
            var result = await _controller.UpdateCustomer("1", updateCustomerDto);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task DeleteCustomer_WithValidId_ReturnsNoContent()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Customers.DeleteCustomerAsync("1"))
                .ReturnsAsync(true);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteCustomer("1");

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCustomer_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Customers.DeleteCustomerAsync("1"))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteCustomer("1");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
} 