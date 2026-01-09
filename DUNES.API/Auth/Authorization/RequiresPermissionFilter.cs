using DUNES.API.ServicesWMS.Auth;
using DUNES.Shared.Utils.Reponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace DUNES.API.Auth.Authorization
{
    /// <summary>
    /// Authorization filter that checks if the authenticated user has a required permission key.
    /// Returns 401 if not authenticated and 403 if authenticated but missing permission.
    /// </summary>
    public sealed class RequiresPermissionFilter : IAsyncAuthorizationFilter
    {
        private const string CacheKey = "__DUNES_MY_PERMISSIONS__";

        private readonly string _requiredPermissionKey;

      
        private readonly IAuthPermissionService _permissionService;

        /// <summary>
        /// Constructor. The permissionKey is provided by the attribute and the service comes from DI.
        /// </summary>
        public RequiresPermissionFilter(string permissionKey, IAuthPermissionService permissionService)
        {
            _requiredPermissionKey = permissionKey ?? string.Empty;
            _permissionService = permissionService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var ct = context.HttpContext.RequestAborted;

            // 1) Must be authenticated (JWT)
            if (context.HttpContext.User?.Identity == null || !context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new ObjectResult(ApiResponseFactory.Unauthorized<object>("Unauthorized"))
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
                return;
            }

            // 2) Read userId from claims (AspNetUsers.Id)
            var userId =
                context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? context.HttpContext.User.FindFirstValue("sub")
                ?? string.Empty;

            if (string.IsNullOrWhiteSpace(userId))
            {
                context.Result = new ObjectResult(ApiResponseFactory.Unauthorized<object>("Unauthorized"))
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
                return;
            }

            // 3) Get cached permissions (avoid hitting DB multiple times if multiple attributes exist)
            List<string>? permissions = null;

            if (context.HttpContext.Items.TryGetValue(CacheKey, out var cached) && cached is List<string> cachedList)
            {
                permissions = cachedList;
            }
            else
            {
                var resp = await _permissionService.GetMyPermissionsAsync(userId, ct);

                // Si falla, tratamos como forbidden (autenticado pero sin permisos efectivos)
                if (!resp.Success || resp.Data == null)
                {
                    context.Result = new ObjectResult(ApiResponseFactory.Forbidden<object>("Forbidden"))
                    {
                        StatusCode = StatusCodes.Status403Forbidden
                    };
                    return;
                }

                permissions = resp.Data;
                context.HttpContext.Items[CacheKey] = permissions;
            }

            // 4) Check required permission
            var hasPermission = permissions.Any(p =>
                string.Equals(p, _requiredPermissionKey, StringComparison.OrdinalIgnoreCase));

            if (!hasPermission)
            {
                context.Result = new ObjectResult(ApiResponseFactory.Forbidden<object>($"Missing permission: {_requiredPermissionKey}"))
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
                return;
            }
        }
    }

    
}
