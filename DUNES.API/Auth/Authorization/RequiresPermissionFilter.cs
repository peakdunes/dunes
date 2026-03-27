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
        private readonly IAuthUserPermissionService _authUserPermissionService;

        /// <summary>
        /// Constructor. The permissionKey is provided by the attribute and the service comes from DI.
        /// </summary>
        public RequiresPermissionFilter(
            string permissionKey,
            IAuthUserPermissionService authUserPermissionService)
        {
            _requiredPermissionKey = permissionKey ?? string.Empty;
            _authUserPermissionService = authUserPermissionService;
        }

        /// <inheritdoc />
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var ct = context.HttpContext.RequestAborted;

            if (context.HttpContext.User?.Identity == null || !context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new ObjectResult(ApiResponseFactory.Unauthorized<object>("Unauthorized"))
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
                return;
            }

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

            List<string> permissions;

            if (context.HttpContext.Items.TryGetValue(CacheKey, out var cached) &&
                cached is List<string> cachedList)
            {
                permissions = cachedList;
            }
            else
            {
                var resp = await _authUserPermissionService.GetCurrentUserPermissionsAsync(userId, ct);

                if (!resp.Success || resp.Data is null)
                {
                    context.Result = new ObjectResult(ApiResponseFactory.Forbidden<object>("Forbidden"))
                    {
                        StatusCode = StatusCodes.Status403Forbidden
                    };
                    return;
                }

                permissions = resp.Data.Permissions ?? new List<string>();
                context.HttpContext.Items[CacheKey] = permissions;
            }

            var hasPermission = permissions.Any(p =>
                string.Equals(p, _requiredPermissionKey, StringComparison.OrdinalIgnoreCase));

            if (!hasPermission)
            {
                context.Result = new ObjectResult(
                    ApiResponseFactory.Forbidden<object>($"Missing permission: {_requiredPermissionKey}"))
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
        }
    }
}