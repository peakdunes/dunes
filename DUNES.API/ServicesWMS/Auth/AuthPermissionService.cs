using DUNES.API.RepositoriesWMS.Auth;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;

namespace DUNES.API.ServicesWMS.Auth
{
    /// <summary>
    /// Auth permissions service implementation.
    /// </summary>
    public class AuthPermissionService : IAuthPermissionService
    {
        private readonly IAuthPermissionRepository _repo;

        /// <summary>
        /// Constructor (DI).
        /// </summary>
        public AuthPermissionService(IAuthPermissionRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Returns effective permission keys for user.
        /// </summary>
        public async Task<ApiResponse<List<string>>> GetMyPermissionsAsync(string userId, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return ApiResponseFactory.Unauthorized<List<string>>("User is not authenticated.");

            var keys = await _repo.GetEffectivePermissionKeysAsync(userId, ct);

            return ApiResponseFactory.Ok(keys, "Permissions loaded.");
        }
    }
}
