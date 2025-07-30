using DUNES.API.Models.Auth;
using DUNES.API.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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


        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="authService"></param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
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
        /// user api test
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("secure")]
        public IActionResult SecureTest()
        {
            var username = User.Identity?.Name;
            return Ok(new
            {
                message = $"🔐 Access granted. Welcome, {username}!",
                user = username
            });
        }
    }
}
