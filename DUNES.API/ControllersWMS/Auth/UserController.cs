using DUNES.API.ServicesWMS.Auth;
using DUNES.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DUNES.API.ControllersWMS.Auth
{
    /// <summary>
    /// Provides endpoints to manage application users in DUNES.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">User management service.</param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Returns all users.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of users.</returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var response = await _userService.GetAllAsync(ct);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Returns a user by identifier.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>User information.</returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(string id, CancellationToken ct)
        {
            var response = await _userService.GetByIdAsync(id, ct);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="dto">Create user DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Created user information.</returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] UserCreateDTO dto, CancellationToken ct)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                               ?? User.FindFirstValue("sub");

            var response = await _userService.CreateAsync(dto, currentUserId, ct);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="dto">Update user DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Updated user information.</returns>
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UserUpdateDTO dto, CancellationToken ct)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                               ?? User.FindFirstValue("sub");

            var response = await _userService.UpdateAsync(dto, currentUserId, ct);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Resets the password of a user and forces password change on next login.
        /// </summary>
        /// <param name="dto">Reset password DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Operation result.</returns>
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto, CancellationToken ct)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                               ?? User.FindFirstValue("sub");

            var response = await _userService.ResetPasswordAsync(dto, currentUserId, ct);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Activates a user.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Operation result.</returns>
        [HttpPatch("Activate/{userId}")]
        public async Task<IActionResult> Activate(string userId, CancellationToken ct)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                               ?? User.FindFirstValue("sub");

            var response = await _userService.SetActiveAsync(userId, true, currentUserId, ct);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Deactivates a user.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Operation result.</returns>
        [HttpPatch("Deactivate/{userId}")]
        public async Task<IActionResult> Deactivate(string userId, CancellationToken ct)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                               ?? User.FindFirstValue("sub");

            var response = await _userService.SetActiveAsync(userId, false, currentUserId, ct);
            return StatusCode(response.StatusCode, response);
        }


        /// <summary>
        /// Changes the password of the current authenticated user.
        /// </summary>
        /// <param name="dto">Change password DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Operation result.</returns>
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto, CancellationToken ct)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                               ?? User.FindFirstValue("sub");

            var response = await _userService.ChangePasswordAsync(dto, currentUserId ?? string.Empty, ct);
            return StatusCode(response.StatusCode, response);
        }
    }
}
