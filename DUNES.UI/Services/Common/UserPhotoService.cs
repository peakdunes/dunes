using DUNES.UI.Models;
using Microsoft.Extensions.Options;

namespace DUNES.UI.Services.Common
{

    /// <summary>
    /// busca la foto de cada usuario en la carpeta configurada en appsetting
    /// </summary>
    public class UserPhotoService : IUserPhotoService
    {
        private readonly UserPhotoSettings _settings;

        public UserPhotoService(IOptions<UserPhotoSettings> options)
        {
            _settings = options.Value;
        }

        public string GetPhotoUrl(string? userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return GetDefaultPhotoUrl();

            return $"{_settings.BaseUrl}/{userId}{_settings.Extension}";
        }

        public string GetDefaultPhotoUrl()
        {
            return $"{_settings.BaseUrl}/{_settings.DefaultFileName}";
        }
    }
}
