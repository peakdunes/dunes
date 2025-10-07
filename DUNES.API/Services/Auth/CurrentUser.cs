using System.Security.Claims;

namespace DUNES.API.Services.Auth
{
    /// <summary>
    /// User Identification Claims
    /// </summary>
    public class CurrentUser: ICurrentUser
    {
        private readonly IHttpContextAccessor _http;

        /// <summary>
        /// Dependency Injection
        /// </summary>
        /// <param name="http"></param>
        public CurrentUser(IHttpContextAccessor http) => _http = http;

         private ClaimsPrincipal? Principal => _http.HttpContext?.User;

       
        /// <summary>
        /// User is Authentication
        /// </summary>
        public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated ?? false;

        /// <summary>
        /// User Id
        /// </summary>
        public string? UserId =>
            Principal?.FindFirstValue(ClaimTypes.NameIdentifier) ??
            Principal?.FindFirstValue("sub"); // JWT subject


        /// <summary>
        /// User Name
        /// </summary>
        public string? UserName =>
            Principal?.FindFirstValue(ClaimTypes.Name) ??
            Principal?.FindFirstValue("name") ??
            Principal?.FindFirstValue("preferred_username") ??
            Principal?.FindFirstValue("unique_name") ??
            UserId; // fallback


        /// <summary>
        /// User List ROles
        /// </summary>
        public IEnumerable<string> Roles =>
            Principal?.FindAll(ClaimTypes.Role).Select(c => c.Value) ??
            Enumerable.Empty<string>();
    }
}
