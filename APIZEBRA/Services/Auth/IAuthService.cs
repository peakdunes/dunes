using APIZEBRA.Models.Auth;

namespace APIZEBRA.Services.Auth
{
    /// <summary>
    /// access token validation
    /// </summary>
    public interface IAuthService
    {
        Task<(string Token, DateTime Expiration)> LoginAsync(LoginModel model);
    }
}
