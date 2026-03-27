using DUNES.Shared.DTOs.Auth;
using System.Text.Json;
namespace DUNES.UI.Helpers
{
    /// <summary>
    /// Provides access to the authenticated user's permission session data.
    /// </summary>
    public class UserPermissionSessionHelper : IUserPermissionSessionHelper
    {
        private const string SessionKey = "USER_PERMISSIONS";
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPermissionSessionHelper"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">HTTP context accessor.</param>
        public UserPermissionSessionHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc />
        public void Save(UserPermissionSessionDTO data)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext is null || data is null)
                return;

            var json = JsonSerializer.Serialize(data);
            httpContext.Session.SetString(SessionKey, json);
        }

        /// <inheritdoc />
        public UserPermissionSessionDTO? Get()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext is null)
                return null;

            var json = httpContext.Session.GetString(SessionKey);
            if (string.IsNullOrWhiteSpace(json))
                return null;

            return JsonSerializer.Deserialize<UserPermissionSessionDTO>(json);
        }

        /// <inheritdoc />
        public bool HasPermission(string permission)
        {
            if (string.IsNullOrWhiteSpace(permission))
                return false;

            var data = Get();
            return data?.Permissions.Contains(permission) == true;
        }

        /// <inheritdoc />
        public void Clear()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext is null)
                return;

            httpContext.Session.Remove(SessionKey);
        }
    }
}
