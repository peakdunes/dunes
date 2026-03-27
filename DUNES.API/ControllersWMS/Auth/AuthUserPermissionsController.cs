using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Auth;
using DUNES.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Auth
{

    /// <summary>
    /// API controller for direct user-permission management.
    /// </summary>
    [ApiController]
    [Route("api/auth/user-permissions")]
    [Authorize]
    public class AuthUserPermissionsController : BaseController
    {
        private readonly IAuthUserPermissionService _authUserPermissionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthUserPermissionsController"/> class.
        /// </summary>
        /// <param name="authUserPermissionService">User-permission service.</param>
        public AuthUserPermissionsController(IAuthUserPermissionService authUserPermissionService)
        {
            _authUserPermissionService = authUserPermissionService;
        }

        /// <summary>
        /// Gets inherited, direct, and effective permissions for a user.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>User permission bundle.</returns>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(string userId, CancellationToken ct)
        {
            return await HandleApi(ct =>
                        _authUserPermissionService.GetByUserAsync(userId, ct), ct);
        }
     
          

        /// <summary>
        /// Saves direct permissions for a user.
        /// </summary>
        /// <param name="request">Save request.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Operation result.</returns>
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] SaveUserPermissionsDTO request, CancellationToken ct)
        {
            return await HandleApi(ct =>
                            _authUserPermissionService.SaveByUserAsync(request, ct), ct);
            
        }

        /// <summary>
        /// Obtain user permissions
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("me")]
        public async Task<IActionResult> GetMyPermissions(CancellationToken ct)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            return await HandleApi(
                ct => _authUserPermissionService.GetCurrentUserPermissionsAsync(userId ?? string.Empty, ct),
                ct);
        }
    }
}
