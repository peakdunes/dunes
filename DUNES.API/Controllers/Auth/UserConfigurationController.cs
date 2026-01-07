using DUNES.API.Services.Auth;
using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Utils.Reponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DUNES.API.Controllers.Auth
{
    /// <summary>
    /// User Configuration Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserConfigurationController : ControllerBase
    {
        private readonly IUserConfigurationService _service;

        /// <summary>
        /// constructor (DI)
        /// </summary>
        /// <param name="service"></param>
        public UserConfigurationController(IUserConfigurationService service)
        {
            _service = service;
        }

        /// <summary>
        /// helper: get logged user id
        /// </summary>
        /// <returns></returns>
        private string? GetLoggedUserId()
        {
            // Typical claim types used by ASP.NET Identity/JWT
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                   ?? User.FindFirstValue("sub")
                   ?? User.FindFirstValue("userid");
        }

        /// <summary>
        /// Get active configuration for logged user
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveAsync(CancellationToken ct)
        {
            var userId = GetLoggedUserId();
            if (string.IsNullOrWhiteSpace(userId))
                return Ok(ApiResponseFactory.Unauthorized<UserConfigurationReadDto?>("User is not authenticated."));

            var resp = await _service.GetActiveAsync(userId, ct);
            return Ok(resp);
        }

        /// <summary>
        /// Get all configurations for logged user
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("by-user")]
        public async Task<IActionResult> GetByUserAsync(CancellationToken ct)
        {
            var userId = GetLoggedUserId();
            if (string.IsNullOrWhiteSpace(userId))
                return Ok(ApiResponseFactory.Unauthorized<List<UserConfigurationReadDto>>("User is not authenticated."));

            var resp = await _service.GetByUserAsync(userId, ct);
            return Ok(resp);
        }

        /// <summary>
        /// Get configuration by id (must belong to logged user)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("by-id/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id, CancellationToken ct)
        {
            var userId = GetLoggedUserId();
            if (string.IsNullOrWhiteSpace(userId))
                return Ok(ApiResponseFactory.Unauthorized<UserConfigurationReadDto?>("User is not authenticated."));

            var resp = await _service.GetByIdAsync(id, userId, ct);
            return Ok(resp);
        }

        /// <summary>
        /// Create configuration (ownership set from logged user)
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] UserConfigurationCreateDto dto, CancellationToken ct)
        {
            var userId = GetLoggedUserId();
            if (string.IsNullOrWhiteSpace(userId))
                return Ok(ApiResponseFactory.Unauthorized<UserConfigurationReadDto>("User is not authenticated."));

            var resp = await _service.CreateAsync(dto, userId, ct);
            return Ok(resp);
        }

        /// <summary>
        /// Update configuration (must belong to logged user)
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UserConfigurationUpdateDto dto, CancellationToken ct)
        {
            var userId = GetLoggedUserId();
            if (string.IsNullOrWhiteSpace(userId))
                return Ok(ApiResponseFactory.Unauthorized<bool>("User is not authenticated."));

            var resp = await _service.UpdateAsync(dto, userId, ct);
            return Ok(resp);
        }

        /// <summary>
        /// Delete configuration by id (must belong to logged user)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken ct)
        {
            var userId = GetLoggedUserId();
            if (string.IsNullOrWhiteSpace(userId))
                return Ok(ApiResponseFactory.Unauthorized<bool>("User is not authenticated."));

            var resp = await _service.DeleteAsync(id, userId, ct);
            return Ok(resp);
        }

        /// <summary>
        /// Set active configuration (deactivates others)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPut("set-active/{id:int}")]
        public async Task<IActionResult> SetActiveAsync(int id, CancellationToken ct)
        {
            var userId = GetLoggedUserId();
            if (string.IsNullOrWhiteSpace(userId))
                return Ok(ApiResponseFactory.Unauthorized<bool>("User is not authenticated."));

            var resp = await _service.SetActiveAsync(id, userId, ct);
            return Ok(resp);
        }

        /// <summary>
        /// Exists env name for logged user (supports UI validation)
        /// </summary>
        /// <param name="envName"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("exists-env")]
        public async Task<IActionResult> ExistsEnvNameAsync([FromQuery] string envName, [FromQuery] int? excludeId, CancellationToken ct)
        {
            var userId = GetLoggedUserId();
            if (string.IsNullOrWhiteSpace(userId))
                return Ok(ApiResponseFactory.Unauthorized<bool>("User is not authenticated."));

            var resp = await _service.ExistsEnvNameAsync(envName, excludeId, userId, ct);
            return Ok(resp);
        }
    }
}
