using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.Auth
{
    public interface IAuthUIService
    {
        Task<ApiResponse<LoginResponseDto>> LoginAsync(string username,string password, CancellationToken ct);
    }
}
