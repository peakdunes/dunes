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
        /// User authentication for DUNES.API.
        /// </summary>
        /// <remarks>
        /// This endpoint validates the user credentials and returns a **JWT token** if the login is successful.
        ///
        /// **Example request:**  
        /// ```json
        /// {
        ///   "username": "admin@dunes.com",
        ///   "password": "123456"
        /// }
        /// ```
        ///
        /// **Example response (200 OK):**  
        /// ```json
        /// {
        ///   "success": true,
        ///   "message": "Login successful",
        ///   "data": {
        ///       "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        ///       "expiration": "2025-08-01T18:30:00Z"
        ///   }
        /// }
        /// ```
        ///
        /// **Example response (401 Unauthorized):**  
        /// ```json
        /// {
        ///   "success": false,
        ///   "message": "Invalid credentials"
        /// }
        /// ```
        /// </remarks>
        /// <param name="model">The login credentials (email and password).</param>
        /// <response code="200">Login successful, returns a JWT token.</response>
        /// <response code="400">Bad request if required fields are missing.</response>
        /// <response code="401">Unauthorized if the credentials are invalid.</response>
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

    }
}
