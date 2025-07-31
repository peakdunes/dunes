using DUNES.API.Models.Auth;
using System.Security.Claims;

namespace DUNES.API.Services.Auth
{
    /// <summary>
    /// access token validation
    /// </summary>
    public interface IAuthService
    {
        Task<(string Token, DateTime Expiration)> LoginAsync(LoginModel model);

        Task<List<string>> GetRolesFromClaims(ClaimsPrincipal userPrincipal);
    }
}
