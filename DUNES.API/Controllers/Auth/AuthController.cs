using DUNES.API.Models.Auth;
using DUNES.API.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.Controllers.Auth
{
    /// <summary>
    /// Autentication controller
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="authService"></param>
        public AuthController(IAuthService authService, UserManager<IdentityUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }
        /// <summary>
        /// Login access
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            return await Handle(async () =>
            {
                var (token, expiration) = await _authService.LoginAsync(model);
                return new { token, expiration };
            });
        }

        /// <summary>
        /// Test endpoint to verify that authentication and token validation are working.
        /// Requires a valid JWT token to access and returns the logged-in username from the token claims.
        /// </summary>
        /// <remarks>
        /// **Example Response**
        /// 
        /// ```json
        /// {
        ///   "message": "🔐 Access granted. Welcome, hlopez@dbk.com!",
        ///   "user": "user@domain.com"
        /// }
        /// ```
        /// 
        /// - If the token is missing or invalid, the endpoint will return **401 Unauthorized**.
        /// </remarks>
        [Authorize]
        [HttpGet("secure")]
        public IActionResult SecureTest()
        {
            var username = User.Identity?.Name;
            return Ok(new
            {
                message = $"Access granted. Welcome, {username}!",
                user = username
            });
        }
        /// <summary>
        /// 🔒 Gets all roles assigned to the authenticated user.
        /// </summary>
        /// <remarks>
        /// Requires a valid JWT token.  
        /// Returns a JSON array with the roles associated to the logged-in user.
        ///
        /// **Example Response**
        /// ```json
        /// {
        ///   "user": "user@domain.com",
        ///   "roles": ["Admin", "Receiving"]
        /// }
        /// ```
        /// </remarks>
        [Authorize]
        [HttpGet("roles")]
        public async Task<IActionResult> GetUserRoles()
        {
            var username = User.Identity?.Name;

            if (string.IsNullOrEmpty(username))
                return Unauthorized(new { message = "Token does not contain a valid username." });

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                return NotFound(new { message = $"User '{username}' not found." });


            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new { 
                user = username,
                roles = roles
            
            });
        }
    }
}
