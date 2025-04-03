using System.Security.Claims;
using TheLoadingBean.Shared.DTOs;

namespace TheLoadingBeanAPI.Services
{
    public interface IJwtService
    {
        TokenDto GenerateToken(string userId, string email, string role);
        ClaimsPrincipal? ValidateToken(string token);
    }
} 