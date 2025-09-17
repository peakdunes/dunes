using DUNES.API.Models.Auth;
using DUNES.Shared.Models;
using System.Security.Claims;

namespace DUNES.API.Services.Auth
{
    /// <summary>
    /// access token validation
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// User Login
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<(string Token, DateTime Expiration)> LoginAsync(LoginModel model, CancellationToken ct);
        /// <summary>
        /// Obtain all roles assigned for this user
        /// </summary>
        /// <param name="userPrincipal"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<string>>> GetRolesFromClaims(ClaimsPrincipal userPrincipal, CancellationToken ct);
    }
}
