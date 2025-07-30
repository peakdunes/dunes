using DUNES.API.Models.Auth;

namespace DUNES.API.Services.Auth
{
    /// <summary>
    /// access token validation
    /// </summary>
    public interface IAuthService
    {
        Task<(string Token, DateTime Expiration)> LoginAsync(LoginModel model);
    }
}
