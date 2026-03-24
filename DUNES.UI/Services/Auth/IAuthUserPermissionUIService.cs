using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.DTOs.Auth.DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.Auth
{
    public interface IAuthUserPermissionUIService
    {
        Task<ApiResponse<UserPermissionBundleDTO>> GetByUserAsync(string token, string userId, CancellationToken ct);
        Task<ApiResponse<bool>> SaveByUserAsync(string token, SaveUserPermissionsDTO request, CancellationToken ct);
    }
}
