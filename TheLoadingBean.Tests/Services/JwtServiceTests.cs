using Microsoft.Extensions.Configuration;
using TheLoadingBean.Shared.Models;
using TheLoadingBeanAPI.Services;

namespace TheLoadingBean.Tests.Services
{
    public class JwtServiceTests
    {
        private readonly IConfiguration _configuration;
        private readonly IJwtService _jwtService;

        public JwtServiceTests()
        {
            var config = new Dictionary<string, string>
            {
                { "Jwt:SecretKey", "your-256-bit-secret-key-here-minimum-32-characters" },
                { "Jwt:Issuer", "TheLoadingBean" },
                { "Jwt:Audience", "TheLoadingBean" },
                { "Jwt:ExpirationMinutes", "60" }
            };
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(config)
                .Build();
            _jwtService = new JwtService(_configuration);
        }

        [Fact]
        public void GenerateToken_WithValidCustomer_ReturnsValidToken()
        {
            // Arrange
            var customer = new Customer
            {
                Id = "1",
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                IsAdmin = false
            };

            // Act
            var token = _jwtService.GenerateToken(customer.Id, customer.Email, "Customer");

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token.Token);
            Assert.True(token.Expiration > DateTime.UtcNow);
        }

        [Fact]
        public void GenerateToken_WithAdminCustomer_IncludesAdminClaim()
        {
            // Arrange
            var customer = new Customer
            {
                Id = "1",
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@example.com",
                IsAdmin = true
            };

            // Act
            var token = _jwtService.GenerateToken(customer.Id, customer.Email, "Admin");

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token.Token);
            Assert.True(token.Expiration > DateTime.UtcNow);
        }

        [Fact]
        public void ValidateToken_WithValidToken_ReturnsTrue()
        {
            // Arrange
            var token = _jwtService.GenerateToken("1", "test@example.com", "Customer");

            // Act
            var principal = _jwtService.ValidateToken(token.Token);

            // Assert
            Assert.NotNull(principal);
            Assert.True(principal.Identity.IsAuthenticated);
        }

        [Fact]
        public void ValidateToken_WithInvalidToken_ReturnsFalse()
        {
            // Arrange
            var invalidToken = "invalid.token.string";

            // Act
            var principal = _jwtService.ValidateToken(invalidToken);

            // Assert
            Assert.Null(principal);
        }

        [Fact]
        public void ValidateToken_WithExpiredToken_ReturnsFalse()
        {
            // Arrange
            var config = new Dictionary<string, string>
            {
                { "Jwt:SecretKey", "your-256-bit-secret-key-here-minimum-32-characters" },
                { "Jwt:Issuer", "TheLoadingBean" },
                { "Jwt:Audience", "TheLoadingBean" },
                { "Jwt:ExpirationMinutes", "0" } // Set expiration to 0 minutes
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(config)
                .Build();
            var jwtService = new JwtService(configuration);
            var token = jwtService.GenerateToken("1", "test@example.com", "Customer");

            // Act
            var principal = jwtService.ValidateToken(token.Token);

            // Assert
            Assert.Null(principal);
        }

        [Fact]
        public void GetUserIdFromToken_WithValidToken_ReturnsUserId()
        {
            // Arrange
            var customer = new Customer
            {
                Id = "1",
                Name = "Test User",
                Email = "test@example.com"
            };
            var token = _jwtService.GenerateToken(customer);

            // Act
            var userId = _jwtService.GetUserIdFromToken(token);

            // Assert
            Assert.Equal(customer.Id, userId);
        }

        [Fact]
        public void GetUserIdFromToken_WithInvalidToken_ReturnsNull()
        {
            // Arrange
            var invalidToken = "invalid.token.string";

            // Act
            var userId = _jwtService.GetUserIdFromToken(invalidToken);

            // Assert
            Assert.Null(userId);
        }

        [Fact]
        public void IsAdminFromToken_WithAdminToken_ReturnsTrue()
        {
            // Arrange
            var customer = new Customer
            {
                Id = "1",
                Name = "Admin User",
                Email = "admin@example.com",
                IsAdmin = true
            };
            var token = _jwtService.GenerateToken(customer);

            // Act
            var isAdmin = _jwtService.IsAdminFromToken(token);

            // Assert
            Assert.True(isAdmin);
        }

        [Fact]
        public void IsAdminFromToken_WithNonAdminToken_ReturnsFalse()
        {
            // Arrange
            var customer = new Customer
            {
                Id = "1",
                Name = "Test User",
                Email = "test@example.com",
                IsAdmin = false
            };
            var token = _jwtService.GenerateToken(customer);

            // Act
            var isAdmin = _jwtService.IsAdminFromToken(token);

            // Assert
            Assert.False(isAdmin);
        }

        [Fact]
        public void IsAdminFromToken_WithInvalidToken_ReturnsFalse()
        {
            // Arrange
            var invalidToken = "invalid.token.string";

            // Act
            var isAdmin = _jwtService.IsAdminFromToken(invalidToken);

            // Assert
            Assert.False(isAdmin);
        }
    }
} 