using DUNES.API.DTOs.B2B;
using DUNES.API.Models.Auth;
using DUNES.API.Utils.Responses;
using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DUNES.API.Services.Auth
{
    /// <summary>
    /// Authentication services
    /// </summary>
    public class AuthService : IAuthService
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
        public async Task<ApiResponse<List<string>>> GetRolesFromClaims(ClaimsPrincipal userPrincipal, CancellationToken ct)
        {

            // Check for cancellation at the start
            ct.ThrowIfCancellationRequested();

            var roleslist = new List<string>();

            // 1. Verifica que hay un usuario autenticado
            if (userPrincipal?.Identity == null || !userPrincipal.Identity.IsAuthenticated)
                return ApiResponseFactory.NotFound<List<string>>("This user is not authenticated.");

            // 2. Obtiene el username (normalmente el email) desde el claim `name`
            var username = userPrincipal.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return ApiResponseFactory.NotFound<List<string>>("This user is not authenticated.");

            ct.ThrowIfCancellationRequested();

            // 3. Busca el usuario en la base de datos de Identity
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return ApiResponseFactory.NotFound<List<string>>("This user is not authenticated.");

            // 4. Obtiene los roles desde Identity
            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null || !roles.Any())
                return ApiResponseFactory.NotFound<List<string>>("Roles list for this user not found.");

            roleslist = roles.ToList();

            ct.ThrowIfCancellationRequested();

            // 5. Devuelve la lista de roles como List<string>
            return ApiResponseFactory.Ok(roleslist, "Roles available for this user");
        }

        /// <summary>
        /// login validation access
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
      


        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginModel model, CancellationToken ct)
        {

            if (string.IsNullOrEmpty(_configuration["JwtSettings:SecretKey"]))
                return ApiResponseFactory.Error<LoginResponseDto>("JWT Info not found");

            if (string.IsNullOrEmpty(_configuration["JwtSettings:Issuer"]))
                return ApiResponseFactory.Error<LoginResponseDto>("JWT Issuer not found");

            if (string.IsNullOrEmpty(_configuration["JwtSettings:Audience"]))
                return ApiResponseFactory.Error<LoginResponseDto>("JWT Audience not found");

            if (string.IsNullOrEmpty(_configuration["JwtSettings:ExpirationMinutes"]))
                return ApiResponseFactory.Error<LoginResponseDto>("Session Expiration time not found");

            if (!int.TryParse(_configuration["JwtSettings:ExpirationMinutes"], out var expiration) || expiration <= 0)
                return ApiResponseFactory.Error<LoginResponseDto>("Session Expiration time not valid");

            // Validate input
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                return ApiResponseFactory.Unauthorized<LoginResponseDto>("Username and password are required");


            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return ApiResponseFactory.Unauthorized<LoginResponseDto>("Invalid credentials");

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["JwtSettings:ExpirationMinutes"]!)),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            string tokenString = tokenHandler.WriteToken(token);

            return ApiResponseFactory.Ok(new LoginResponseDto
            {
                Token = tokenString,
                Expiration = token.ValidTo
            });
        }
    }
}
