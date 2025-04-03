using Microsoft.AspNetCore.Components.Authorization;
using Moq;
using TheLoadingBean.Client.Services;
using TheLoadingBean.Shared.DTOs;

namespace TheLoadingBean.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<HttpClient> _mockHttpClient;
        private readonly Mock<ILocalStorageService> _mockLocalStorageService;
        private readonly Mock<IToastService> _mockToastService;
        private readonly Mock<AuthenticationStateProvider> _mockAuthStateProvider;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockHttpClient = new Mock<HttpClient>();
            _mockLocalStorageService = new Mock<ILocalStorageService>();
            _mockToastService = new Mock<IToastService>();
            _mockAuthStateProvider = new Mock<AuthenticationStateProvider>();
            _authService = new AuthService(
                _mockHttpClient.Object,
                _mockLocalStorageService.Object,
                _mockToastService.Object,
                _mockAuthStateProvider.Object);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsToken()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "test@example.com",
                Password = "Password123!"
            };
            var token = new TokenDto
            {
                Token = "test.jwt.token",
                Expiration = DateTime.UtcNow.AddHours(1)
            };
            _mockHttpClient.Setup(h => h.PostAsJsonAsync(It.IsAny<string>(), loginDto))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK, Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(token)) });

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(token.Token, result.Token);
            Assert.Equal(token.Expiration, result.Expiration);
            _mockLocalStorageService.Verify(s => s.SetItemAsync("authToken", token.Token), Times.Once);
            _mockLocalStorageService.Verify(s => s.SetItemAsync("tokenExpiration", token.Expiration), Times.Once);
            _mockToastService.Verify(t => t.ShowSuccess("Successfully logged in!"), Times.Once);
        }

        [Fact]
        public async Task Register_WithValidData_ReturnsToken()
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
            var token = new TokenDto
            {
                Token = "test.jwt.token",
                Expiration = DateTime.UtcNow.AddHours(1)
            };
            _mockHttpClient.Setup(h => h.PostAsJsonAsync(It.IsAny<string>(), registerDto))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK, Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(token)) });

            // Act
            var result = await _authService.RegisterAsync(registerDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(token.Token, result.Token);
            Assert.Equal(token.Expiration, result.Expiration);
            _mockLocalStorageService.Verify(s => s.SetItemAsync("authToken", token.Token), Times.Once);
            _mockLocalStorageService.Verify(s => s.SetItemAsync("tokenExpiration", token.Expiration), Times.Once);
            _mockToastService.Verify(t => t.ShowSuccess("Successfully registered!"), Times.Once);
        }

        [Fact]
        public async Task Logout_ClearsStorageAndNotifiesStateChange()
        {
            // Arrange
            _mockLocalStorageService.Setup(s => s.RemoveItemAsync("authToken"))
                .Returns(Task.CompletedTask);
            _mockLocalStorageService.Setup(s => s.RemoveItemAsync("tokenExpiration"))
                .Returns(Task.CompletedTask);

            // Act
            await _authService.LogoutAsync();

            // Assert
            _mockLocalStorageService.Verify(s => s.RemoveItemAsync("authToken"), Times.Once);
            _mockLocalStorageService.Verify(s => s.RemoveItemAsync("tokenExpiration"), Times.Once);
            _mockToastService.Verify(t => t.ShowSuccess("Successfully logged out!"), Times.Once);
        }

        [Fact]
        public async Task IsAuthenticatedAsync_WithValidToken_ReturnsTrue()
        {
            // Arrange
            var token = "test.jwt.token";
            var expiration = DateTime.UtcNow.AddHours(1);
            _mockLocalStorageService.Setup(s => s.GetItemAsync<string>("authToken"))
                .ReturnsAsync(token);
            _mockLocalStorageService.Setup(s => s.GetItemAsync<DateTime>("tokenExpiration"))
                .ReturnsAsync(expiration);

            // Act
            var result = await _authService.IsAuthenticatedAsync();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsAuthenticatedAsync_WithExpiredToken_ReturnsFalse()
        {
            // Arrange
            var token = "test.jwt.token";
            var expiration = DateTime.UtcNow.AddHours(-1);
            _mockLocalStorageService.Setup(s => s.GetItemAsync<string>("authToken"))
                .ReturnsAsync(token);
            _mockLocalStorageService.Setup(s => s.GetItemAsync<DateTime>("tokenExpiration"))
                .ReturnsAsync(expiration);

            // Act
            var result = await _authService.IsAuthenticatedAsync();

            // Assert
            Assert.False(result);
            _mockLocalStorageService.Verify(s => s.RemoveItemAsync("authToken"), Times.Once);
            _mockLocalStorageService.Verify(s => s.RemoveItemAsync("tokenExpiration"), Times.Once);
        }

        [Fact]
        public async Task IsAuthenticatedAsync_WithNoToken_ReturnsFalse()
        {
            // Arrange
            _mockLocalStorageService.Setup(s => s.GetItemAsync<string>("authToken"))
                .ReturnsAsync((string)null);

            // Act
            var result = await _authService.IsAuthenticatedAsync();

            // Assert
            Assert.False(result);
        }
    }
} 