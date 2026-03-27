using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.Auth
{
    /// <summary>
    /// Provides UI operations related to the authenticated user's effective permissions.
    /// </summary>
    public class CurrentUserPermissionUIService : UIApiServiceBase, ICurrentUserPermissionUIService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentUserPermissionUIService"/> class.
        /// </summary>
        /// <param name="factory">HTTP client factory.</param>
        public CurrentUserPermissionUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        /// <inheritdoc />
        public async Task<ApiResponse<CurrentUserPermissionsDTO>> GetMyPermissionsAsync(string token, CancellationToken ct)
        {
            return await GetApiAsync<CurrentUserPermissionsDTO>(
                "/api/auth/user-permissions/me",
                token,
                ct
            );
        }
    }
}
