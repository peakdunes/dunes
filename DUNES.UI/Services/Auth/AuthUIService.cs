using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.Auth
{
    public class AuthUIService : UIApiServiceBase, IAuthUIService
    {

        /// <summary>
        /// Authentication Service
        /// </summary>
        /// <param name="factory"></param>
        public AuthUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }


        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(string username, string password, CancellationToken ct)
        {
            var body = new
            {
                username,
                password
            };

            var infoaut = await PostApiAsync<LoginResponseDto, object>(
               "/api/Auth/login",
               body,
               token: string.Empty, // login no requiere token
               ct);

            return await PostApiAsync<LoginResponseDto, object>(
               "/api/Auth/login",
               body,
               token: string.Empty, // login no requiere token
               ct
           );
        }
    }
}
