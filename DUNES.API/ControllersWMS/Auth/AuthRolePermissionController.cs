using DUNES.API.ServicesWMS.Auth;
using DUNES.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Auth
{
    /// <summary>
    /// Provides endpoints to manage role-permission assignments.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthRolePermissionController : ControllerBase
    {
        private readonly IAuthRolePermissionService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthRolePermissionController"/> class.
        /// </summary>
        /// <param name="service">Role-permission service.</param>
        public AuthRolePermissionController(IAuthRolePermissionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Returns all permissions and indicates which are assigned to the specified role.
        /// </summary>
        /// <param name="roleId">Role identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of role permission items.</returns>
        [HttpGet("GetByRole/{roleId}")]
        public async Task<IActionResult> GetByRole(string roleId, CancellationToken ct)
        {
            var response = await _service.GetByRoleAsync(roleId, ct);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Saves the complete permission set for the specified role.
        /// </summary>
        /// <param name="dto">Save request DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Operation result.</returns>
        [HttpPut("SaveByRole")]
        public async Task<IActionResult> SaveByRole([FromBody] SaveRolePermissionsDTO dto, CancellationToken ct)
        {
            var response = await _service.SaveByRoleAsync(dto, ct);
            return StatusCode(response.StatusCode, response);
        }
    }
}