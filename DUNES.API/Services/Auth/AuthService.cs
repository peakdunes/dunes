using DUNES.API.DTOs.B2B;
using DUNES.API.Models.Auth;
using DUNES.API.ModelsWMS.Auth;
using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DUNES.API.Services.Auth
{
    /// <summary>
    /// Authentication services for login validation and role retrieval.
    /// Uses ApplicationUser as the Identity user entity for DUNES.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserConfigurationService _userConfigurationService;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="userManager">Identity user manager for ApplicationUser.</param>
        /// <param name="configuration">Application configuration.</param>
        /// <param name="userConfigurationService">Service used to obtain the active user configuration.</param>
        public AuthService(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IUserConfigurationService userConfigurationService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _userConfigurationService = userConfigurationService;
        }

        /// <summary>
        /// Obtains all roles assigned to the currently authenticated user.
        /// </summary>
        /// <param name="userPrincipal">Authenticated user principal.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of role names assigned to the user.</returns>
        public async Task<ApiResponse<List<string>>> GetRolesFromClaims(ClaimsPrincipal userPrincipal, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var rolesList = new List<string>();

            if (userPrincipal?.Identity == null || !userPrincipal.Identity.IsAuthenticated)
                return ApiResponseFactory.NotFound<List<string>>("This user is not authenticated.");

            var username = userPrincipal.Identity.Name;
            if (string.IsNullOrWhiteSpace(username))
                return ApiResponseFactory.NotFound<List<string>>("This user is not authenticated.");

            ct.ThrowIfCancellationRequested();

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return ApiResponseFactory.NotFound<List<string>>("This user is not authenticated.");

            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null || !roles.Any())
                return ApiResponseFactory.NotFound<List<string>>("Roles list for this user not found.");

            rolesList = roles.ToList();

            ct.ThrowIfCancellationRequested();

            return ApiResponseFactory.Ok(rolesList, "Roles available for this user");
        }

        /// <summary>
        /// Validates user credentials and generates a JWT token for the authenticated user.
        /// Also validates whether the user is active and returns the MustChangePassword flag.
        /// </summary>
        /// <param name="model">Login request model containing username and password.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Login response with JWT token, expiration, user context, and password-change requirement flag.</returns>
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

            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
                return ApiResponseFactory.Unauthorized<LoginResponseDto>("Username and password are required");

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return ApiResponseFactory.Unauthorized<LoginResponseDto>("Invalid credentials");

            if (!user.IsActive)
                return ApiResponseFactory.Unauthorized<LoginResponseDto>("This user is inactive.");

            var userConfig = await _userConfigurationService.GetActiveAsync(user.Id, ct);

            var hasConfiguration = userConfig.Success && userConfig.Data != null;

            var userRoles = await _userManager.GetRolesAsync(user);


            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (hasConfiguration)
            {
                authClaims.Add(new Claim("companyId", userConfig.Data.Companydefault.ToString()));
                authClaims.Add(new Claim("companyClientId", userConfig.Data.Companyclientdefault.ToString()));
                authClaims.Add(new Claim("locationId", userConfig.Data.Locationdefault.ToString()));
                authClaims.Add(new Claim("companiesContractId", userConfig.Data.companiesContractId.ToString()));
            }

          

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                expires: DateTime.UtcNow.AddMinutes(expiration),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);

            var response = new LoginResponseDto
            {
                UserId = user.Id,
                Token = tokenString,
                Expiration = token.ValidTo,
                UserName = user.UserName ?? string.Empty,

                MustChangePassword = user.MustChangePassword,
                HasConfiguration = hasConfiguration
            };

            if (hasConfiguration)
            {
                response.CompanyId = userConfig.Data.Companydefault;
                response.CompanyClientId = userConfig.Data.Companyclientdefault;
                response.LocationId = userConfig.Data.Locationdefault;

                response.companyName = userConfig.Data.CompanyName;
                response.companyClientName = userConfig.Data.CompanyClientName;
                response.LocationName = userConfig.Data.LocationName;
                response.Enviromentname = userConfig.Data.Enviromentname;

                response.RoleName = userConfig.Data.RoleName;
                response.companiesContractId = userConfig.Data.companiesContractId;


            }
            return ApiResponseFactory.Ok(response);

          
        }
    }
}