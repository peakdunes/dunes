using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Auth
{
    /// <summary>
    /// Auth permissions service interface.
    /// </summary>
    public interface IAuthPermissionService
    {
        /// <summary>
        /// Returns effective permissions for logged user.
        /// </summary>
        Task<ApiResponse<List<string>>> GetMyPermissionsAsync(string userId, CancellationToken ct);
    }
}
