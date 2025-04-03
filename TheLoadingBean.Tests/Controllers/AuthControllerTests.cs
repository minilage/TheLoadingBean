using Microsoft.AspNetCore.Mvc;
using Moq;
using TheLoadingBean.Shared.DTOs;
using TheLoadingBean.Shared.Models;
using TheLoadingBeanAPI.Controllers;
using TheLoadingBeanAPI.Data;
using TheLoadingBeanAPI.Services;
using System.Security.Claims;

namespace TheLoadingBean.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IJwtService> _mockJwtService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockJwtService = new Mock<IJwtService>();
            _controller = new AuthController(_mockUnitOfWork.Object, _mockJwtService.Object);
        }

        [Fact]
        public async Task Register_WithValidData_ReturnsOkResult_WithToken()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                Password = "Password123!",
                Phone = "1234567890",
                Address = "Test Address",
                ConfirmPassword = "Password123!"
            };
            var customer = new Customer
            {
                Id = "1",
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                Phone = registerDto.Phone,
                Address = registerDto.Address
            };
            var token = new TokenDto
            {
                Token = "test.jwt.token",
                Expiration = DateTime.UtcNow.AddHours(1)
            };
            _mockUnitOfWork.Setup(u => u.Customers.GetCustomerByEmailAsync(registerDto.Email))
                .ReturnsAsync((Customer)null);
            _mockUnitOfWork.Setup(u => u.Customers.CreateCustomerAsync(It.IsAny<Customer>()))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync())
                .Returns(Task.CompletedTask);
            _mockJwtService.Setup(j => j.GenerateToken(customer.Id, customer.Email, "Customer"))
                .Returns(token);

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<TokenDto>(okResult.Value);
            Assert.Equal(token.Token, response.Token);
            Assert.Equal(token.Expiration, response.Expiration);
        }

        [Fact]
        public async Task Register_WithExistingEmail_ReturnsBadRequest()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Email = "existing@example.com"
            };
            var existingCustomer = new Customer { Email = registerDto.Email };
            _mockUnitOfWork.Setup(u => u.Customers.GetCustomerByEmailAsync(registerDto.Email))
                .ReturnsAsync(existingCustomer);

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("A user with this email already exists.", badRequestResult.Value);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsOkResult_WithToken()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "test@example.com",
                Password = "Password123!"
            };
            var customer = new Customer
            {
                Id = "1",
                FirstName = "Test",
                LastName = "User",
                Email = loginDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(loginDto.Password)
            };
            var token = new TokenDto
            {
                Token = "test.jwt.token",
                Expiration = DateTime.UtcNow.AddHours(1)
            };
            _mockUnitOfWork.Setup(u => u.Customers.GetCustomerByEmailAsync(loginDto.Email))
                .ReturnsAsync(customer);
            _mockJwtService.Setup(j => j.GenerateToken(customer.Id, customer.Email, "Customer"))
                .Returns(token);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<TokenDto>(okResult.Value);
            Assert.Equal(token.Token, response.Token);
            Assert.Equal(token.Expiration, response.Expiration);
        }

        [Fact]
        public async Task Login_WithInvalidEmail_ReturnsUnauthorized()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "nonexistent@example.com"
            };
            _mockUnitOfWork.Setup(u => u.Customers.GetCustomerByEmailAsync(loginDto.Email))
                .ReturnsAsync((Customer)null);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            Assert.IsType<UnauthorizedResult>(result.Result);
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ReturnsUnauthorized()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "test@example.com",
                Password = "WrongPassword"
            };
            var customer = new Customer
            {
                Email = loginDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword")
            };
            _mockUnitOfWork.Setup(u => u.Customers.GetCustomerByEmailAsync(loginDto.Email))
                .ReturnsAsync(customer);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            Assert.IsType<UnauthorizedResult>(result.Result);
        }

        [Fact]
        public async Task GetCurrentUser_WithValidToken_ReturnsOkResult_WithUser()
        {
            // Arrange
            var customer = new Customer
            {
                Id = "1",
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                Phone = "1234567890",
                Address = "Test Address"
            };
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, customer.Id),
                new Claim(ClaimTypes.Email, customer.Email),
                new Claim(ClaimTypes.Role, "Customer")
            };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };
            _mockUnitOfWork.Setup(u => u.Customers.GetCustomerByIdAsync("1"))
                .ReturnsAsync(customer);

            // Act
            var result = await _controller.GetCurrentUser();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<CustomerResponseDto>(okResult.Value);
            Assert.Equal(customer.Id, response.Id);
            Assert.Equal(customer.FirstName, response.FirstName);
            Assert.Equal(customer.LastName, response.LastName);
            Assert.Equal(customer.Email, response.Email);
            Assert.Equal(customer.Phone, response.Phone);
            Assert.Equal(customer.Address, response.Address);
            Assert.False(response.IsAdmin);
        }

        [Fact]
        public async Task GetCurrentUser_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.Role, "Customer")
            };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };
            _mockUnitOfWork.Setup(u => u.Customers.GetCustomerByIdAsync("1"))
                .ReturnsAsync((Customer)null);

            // Act
            var result = await _controller.GetCurrentUser();

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetCurrentUser_WithInvalidToken_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            // Act
            var result = await _controller.GetCurrentUser();

            // Assert
            Assert.IsType<UnauthorizedResult>(result.Result);
        }
    }
} 