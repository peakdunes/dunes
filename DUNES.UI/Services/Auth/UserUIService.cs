using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.Auth
{
    public class UserUIService : UIApiServiceBase, IUserUIService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserUIService(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor)
            : base(factory)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<List<UserReadDTO>>> GetAllAsync(CancellationToken ct)
        {
            return await GetApiAsync<List<UserReadDTO>>(
                "/api/User/GetAll",
                token: _httpContextAccessor.HttpContext?.Request.Cookies["api_token"] ?? string.Empty,
                ct
            );
        }
    }
}
