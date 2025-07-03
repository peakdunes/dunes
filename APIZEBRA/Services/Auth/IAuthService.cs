using APIZEBRA.Models.Auth;

namespace APIZEBRA.Services.Auth
{
    public interface IAuthService
    {
        Task<(string Token, DateTime Expiration)> LoginAsync(LoginModel model);
    }
}
