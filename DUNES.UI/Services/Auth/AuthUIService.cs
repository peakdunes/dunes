using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.Auth
{
    public class AuthUIService : UIApiServiceBase, IAuthUIService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Authentication Service
        /// </summary>
        /// <param name="factory"></param>
        public AuthUIService(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor)
      : base(factory)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(string username, string password, CancellationToken ct)
        {
            var body = new
            {
                username,
                password
            };

            return await PostApiAsync<LoginResponseDto, object>(
                "/api/Auth/login",
                body,
                token: string.Empty,
                ct
            );
        }

        /// <summary>
        /// Sends a change password request to the API.
        /// </summary>
        /// <param name="dto">Change password DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Operation result.</returns>
        /// <summary>
        /// Sends a change password request to the API.
        /// </summary>
        /// <param name="dto">Change password DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Operation result.</returns>
        public async Task<ApiResponse<bool>> ChangePasswordAsync(ChangePasswordDTO dto, CancellationToken ct)
        {
            return await PostApiAsync<bool, object>(
                "/api/User/ChangePassword",
                dto,
                token: _httpContextAccessor.HttpContext?.Request.Cookies["api_token"] ?? string.Empty,
                ct
            );
        }

    }
}
