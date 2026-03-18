using DUNES.API.Models.Auth;
using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using System.Security.Claims;

namespace DUNES.API.Services.Auth
{
    /// <summary>
    /// Defines authentication and login operations for DUNES.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Obtains all roles assigned to the currently authenticated user.
        /// </summary>
        /// <param name="userPrincipal">Authenticated user principal.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of role names assigned to the current user.</returns>
        Task<ApiResponse<List<string>>> GetRolesFromClaims(ClaimsPrincipal userPrincipal, CancellationToken ct);

        /// <summary>
        /// Validates user credentials and returns the login response with token and user context.
        /// </summary>
        /// <param name="model">Login request model.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Login response including JWT token and current user context.</returns>
        Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginModel model, CancellationToken ct);
    }
}