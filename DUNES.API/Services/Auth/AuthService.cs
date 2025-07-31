using DUNES.API.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DUNES.API.Services.Auth
{
    /// <summary>
    /// Authentication services
    /// </summary>
    public class AuthService: IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="configuration"></param>
        public AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Obtains all roles for the currently authenticated user using the claims in the JWT token.
        /// </summary>
        /// <param name="userPrincipal">The ClaimsPrincipal object injected by ASP.NET (User)</param>
        /// <returns>List of role names (or empty list if no roles)</returns>
        public async Task<List<string>> GetRolesFromClaims(ClaimsPrincipal userPrincipal)
        {
            // 1. Verifica que hay un usuario autenticado
            if (userPrincipal?.Identity == null || !userPrincipal.Identity.IsAuthenticated)
                return new List<string>();

            // 2. Obtiene el username (normalmente el email) desde el claim `name`
            var username = userPrincipal.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return new List<string>();

            // 3. Busca el usuario en la base de datos de Identity
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return new List<string>();

            // 4. Obtiene los roles desde Identity
            var roles = await _userManager.GetRolesAsync(user);

            // 5. Devuelve la lista de roles como List<string>
            return roles.ToList();
        }

        /// <summary>
        /// login validation access
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public async Task<(string Token, DateTime Expiration)> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                throw new UnauthorizedAccessException("Invalid credentials");

            var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["JwtSettings:ExpirationMinutes"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
        }
    }
}
