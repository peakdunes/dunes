using APIZEBRA.Models.Auth;
using APIZEBRA.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIZEBRA.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            return await Handle(async () =>
            {
                var (token, expiration) = await _authService.LoginAsync(model);
                return new { token, expiration };
            });
        }


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
