using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.Auth
{
    public interface IAuthUIService
    {
        Task<ApiResponse<LoginResponseDto>> LoginAsync(string username,string password, CancellationToken ct);


        /// <summary>
        /// Sends a change password request to the API.
        /// </summary>
        /// <param name="dto">Change password DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Operation result.</returns>
        Task<ApiResponse<bool>> ChangePasswordAsync(ChangePasswordDTO dto, CancellationToken ct);
    }
}
