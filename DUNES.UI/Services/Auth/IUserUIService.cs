using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.Auth
{
    public interface IUserUIService
    {
        Task<ApiResponse<List<UserReadDTO>>> GetAllAsync(CancellationToken ct);
    }
}
