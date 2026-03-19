using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.Auth
{
    public interface IUserUIService
    {
        Task<ApiResponse<List<UserReadDTO>>> GetAllAsync(string token, CancellationToken ct);
        Task<ApiResponse<UserReadDTO>> GetByIdAsync(string token, string id, CancellationToken ct);
        Task<ApiResponse<UserReadDTO>> CreateAsync(string token, UserCreateDTO dto, CancellationToken ct);
        Task<ApiResponse<UserReadDTO>> UpdateAsync(string token, UserUpdateDTO dto, CancellationToken ct);
        Task<ApiResponse<bool>> ActivateAsync(string token, string userId, CancellationToken ct);
        Task<ApiResponse<bool>> DeactivateAsync(string token, string userId, CancellationToken ct);
        Task<ApiResponse<bool>> ResetPasswordAsync(string token, ResetPasswordDTO dto, CancellationToken ct);
        Task<ApiResponse<bool>> ChangePasswordAsync(string token, ChangePasswordDTO dto, CancellationToken ct);
        Task<ApiResponse<List<RoleOptionDTO>>> GetRolesAsync(string token, CancellationToken ct);
    }
}
