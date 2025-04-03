using Blazored.LocalStorage;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using TheLoadingBean.Client.Auth;
using TheLoadingBean.Shared.DTOs;

namespace TheLoadingBean.Client.Services
{
    public interface IAuthService
    {
        Task<TokenDto> LoginAsync(LoginDto loginDto);
        Task<TokenDto> RegisterAsync(RegisterDto registerDto);
        Task LogoutAsync();
        bool IsAuthenticated { get; }
        bool IsAdmin { get; }
        string UserId { get; }
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly IToastService _toastService;
        private readonly CustomAuthStateProvider _authStateProvider;
        private readonly string _baseUrl = "api/auth/";

        public AuthService(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            IToastService toastService,
            AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _toastService = toastService;
            _authStateProvider = (CustomAuthStateProvider)authStateProvider;
        }

        public async Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}login", loginDto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _toastService.ShowError($"Inloggning misslyckades: {error}");
                throw new Exception("Login failed");
            }

            var token = await response.Content.ReadFromJsonAsync<TokenDto>();
            if (token is null)
                throw new Exception("Token is null");

            await _localStorage.SetItemAsync("authToken", token.Token);
            await _localStorage.SetItemAsync("tokenExpiration", token.Expiration);
            _authStateProvider.NotifyUserAuthentication(token.Token);
            _toastService.ShowSuccess("Inloggning lyckades!");

            return token;
        }

        public async Task<TokenDto> RegisterAsync(RegisterDto registerDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}register", registerDto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _toastService.ShowError($"Registrering misslyckades: {error}");
                throw new Exception("Registration failed");
            }

            var token = await response.Content.ReadFromJsonAsync<TokenDto>();
            if (token is null)
                throw new Exception("Token is null");

            await _localStorage.SetItemAsync("authToken", token.Token);
            await _localStorage.SetItemAsync("tokenExpiration", token.Expiration);
            _authStateProvider.NotifyUserAuthentication(token.Token);
            _toastService.ShowSuccess("Registrering lyckades!");

            return token;
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("tokenExpiration");
            _authStateProvider.NotifyUserLogout();
            _toastService.ShowSuccess("Utloggning lyckades!");
        }

        public bool IsAuthenticated => _authStateProvider.IsAuthenticated;
        public bool IsAdmin => _authStateProvider.IsAdmin;
        public string UserId => _authStateProvider.UserId;
    }
}
