using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Auth;
using DUNES.Shared.Utils.Reponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DUNES.API.ControllersWMS.Auth
{
    /// <summary>
    /// Auth Permissions Service
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // JWT required
    public class AuthPermissionsController : BaseController
    {
        private readonly IAuthPermissionService _service;


        /// <summary>
        /// Constructor (DI)
        /// </summary>
        /// <param name="service"></param>
        public AuthPermissionsController(IAuthPermissionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Returns effective permissions for the logged user (role permissions + user grants).
        /// </summary>
        [HttpGet("my-permissions")]
        public async Task<IActionResult> MyPermissions(CancellationToken ct)
        {
            return await HandleApi(async ct =>
            {

                //esta instruccion lee los claims que vienen en eltoken para obtener el user id 
                // AspNetUsers.Id (GUID) viene en NameIdentifier
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)   //aqui sabe cual es el usuario porque lo lee de los
                                                                              //claims que mando el cliente en la peticion
                          ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                          ?? User.FindFirstValue("sub")
                          ?? string.Empty;

                return await _service.GetMyPermissionsAsync(userId, ct);
            }, ct);
        }
        /// <summary>
        /// Debug endpoint that returns the JWT claims currently attached to the authenticated request.
        /// Useful to confirm which claim types/values are being emitted by the Login token
        /// (e.g., NameIdentifier, sub, roles) when troubleshooting authorization/permissions.
        /// </summary>
        /// <param name="ct">Cancellation token propagated from the HTTP request.</param>
        /// <returns>
        /// ApiResponse with a list of objects { type, value } representing the current User.Claims.
        /// </returns>

        [HttpGet("debug-claims")]
        [Authorize]
        public async Task<IActionResult> DebugClaims(CancellationToken ct)
        {
            return await HandleApi(ct =>
            {
                var claims = User.Claims
                    .Select(c => new { type = c.Type, value = c.Value })
                    .OrderBy(x => x.type)
                    .ToList();

                return Task.FromResult(ApiResponseFactory.Ok(claims, "JWT claims loaded."));
            }, ct);
        }
    }
}
