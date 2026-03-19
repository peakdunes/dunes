using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.Auth
{
    /// <summary>
    /// Service implementation for permission catalog operations from the MVC UI.
    /// </summary>
    public class AuthPermissionUIService : UIApiServiceBase, IAuthPermissionUIService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthPermissionUIService"/> class.
        /// </summary>
        /// <param name="factory">HTTP client factory.</param>
        public AuthPermissionUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        /// <summary>
        /// Retrieves all permissions from the catalog.
        /// </summary>
        /// <param name="token">Authenticated API token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of permissions.</returns>
        public async Task<ApiResponse<List<AuthPermissionReadDTO>>> GetAllAsync(string token, CancellationToken ct)
        {
            return await GetApiAsync<List<AuthPermissionReadDTO>>(
                "/api/AuthPermission/GetAll",
                token,
                ct
            );
        }

        /// <summary>
        /// Creates a new permission in the catalog.
        /// </summary>
        /// <param name="token">Authenticated API token.</param>
        /// <param name="dto">Permission creation DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created permission record.</returns>
        public async Task<ApiResponse<AuthPermissionReadDTO>> CreateAsync(string token, AuthPermissionCreateDTO dto, CancellationToken ct)
        {
            return await PostApiAsync<AuthPermissionReadDTO, AuthPermissionCreateDTO>(
                "/api/AuthPermission/Create",
                dto,
                token,
                ct
            );
        }
    }
}
