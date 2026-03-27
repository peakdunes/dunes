using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.Auth
{
    /// <summary>
    /// Provides UI operations for user management.
    /// </summary>
    public class UserUIService : UIApiServiceBase, IUserUIService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserUIService"/> class.
        /// </summary>
        /// <param name="factory">HTTP client factory.</param>
        public UserUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        /// <inheritdoc />
        public async Task<ApiResponse<List<UserReadDTO>>> GetAllAsync(string token, CancellationToken ct)
        {
            return await GetApiAsync<List<UserReadDTO>>(
                "/api/User/GetAll",
                token,
                ct);
        }

        /// <inheritdoc />
        public async Task<ApiResponse<UserReadDTO>> GetByIdAsync(string token, string id, CancellationToken ct)
        {
            return await GetApiAsync<UserReadDTO>(
                $"/api/User/GetById/{id}",
                token,
                ct);
        }

        /// <inheritdoc />
        public async Task<ApiResponse<UserReadDTO>> CreateAsync(string token, UserCreateDTO dto, CancellationToken ct)
        {
            return await PostApiAsync<UserReadDTO, UserCreateDTO>(
                "/api/User/Create",
                dto,
                token,
                ct);
        }

        /// <inheritdoc />
        public async Task<ApiResponse<UserReadDTO>> UpdateAsync(string token, UserUpdateDTO dto, CancellationToken ct)
        {
            return await PutApiAsync<UserReadDTO, UserUpdateDTO>(
                "/api/User/Update",
                dto,
                token,
                ct);
        }

        /// <inheritdoc />
        public async Task<ApiResponse<bool>> ActivateAsync(string token, string userId, CancellationToken ct)
        {
            return await PatchApiAsync<bool>(
                $"/api/User/Activate/{userId}",
                token,
                ct);
        }

        /// <inheritdoc />
        public async Task<ApiResponse<bool>> DeactivateAsync(string token, string userId, CancellationToken ct)
        {
            return await PatchApiAsync<bool>(
                $"/api/User/Deactivate/{userId}",
                token,
                ct);
        }

        /// <inheritdoc />
        public async Task<ApiResponse<bool>> ResetPasswordAsync(string token, ResetPasswordDTO dto, CancellationToken ct)
        {
            return await PostApiAsync<bool, ResetPasswordDTO>(
                "/api/User/ResetPassword",
                dto,
                token,
                ct);
        }

        /// <inheritdoc />
        public async Task<ApiResponse<bool>> ChangePasswordAsync(string token, ChangePasswordDTO dto, CancellationToken ct)
        {
            return await PostApiAsync<bool, ChangePasswordDTO>(
                "/api/User/ChangePassword",
                dto,
                token,
                ct);
        }

        /// <inheritdoc />
        public async Task<ApiResponse<List<RoleOptionDTO>>> GetRolesAsync(string token, CancellationToken ct)
        {
            return await GetApiAsync<List<RoleOptionDTO>>(
                "/api/User/GetRoles",
                token,
                ct);
        }
    }
}