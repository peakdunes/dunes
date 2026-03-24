using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.DTOs.Auth.DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using System.Net.Http.Headers;

namespace DUNES.UI.Services.Auth
{
    public class AuthUserPermissionUIService : IAuthUserPermissionUIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AuthUserPermissionUIService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<ApiResponse<UserPermissionBundleDTO>> GetByUserAsync(string token, string userId, CancellationToken ct)
        {
            var client = CreateClient(token);

            var response = await client.GetAsync($"api/auth/user-permissions/user/{userId}", ct);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ApiResponse<UserPermissionBundleDTO>>(cancellationToken: ct);
                return error ?? ApiResponseFactory.Fail<UserPermissionBundleDTO>(
                    error: "HTTP_ERROR",
                    message: $"Request failed with status code {(int)response.StatusCode}.",
                    statusCode: (int)response.StatusCode);
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<UserPermissionBundleDTO>>(cancellationToken: ct);

            return result ?? ApiResponseFactory.Fail<UserPermissionBundleDTO>(
                error: "NULL_RESPONSE",
                message: "The API returned an empty response.",
                statusCode: 500);
        }

        public async Task<ApiResponse<bool>> SaveByUserAsync(string token, SaveUserPermissionsDTO request, CancellationToken ct)
        {
            var client = CreateClient(token);

            var response = await client.PostAsJsonAsync("api/auth/user-permissions/save", request, ct);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>(cancellationToken: ct);
                return error ?? ApiResponseFactory.Fail<bool>(
                    error: "HTTP_ERROR",
                    message: $"Request failed with status code {(int)response.StatusCode}.",
                    statusCode: (int)response.StatusCode);
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>(cancellationToken: ct);

            return result ?? ApiResponseFactory.Fail<bool>(
                error: "NULL_RESPONSE",
                message: "The API returned an empty response.",
                statusCode: 500);
        }

        private HttpClient CreateClient(string token)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["ApiSettings:BaseUrl"]!);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }
    }
}
