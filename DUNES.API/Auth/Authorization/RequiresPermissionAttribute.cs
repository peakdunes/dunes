using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.Auth.Authorization
{
    /// <summary>
    /// Requires a specific permission key for the endpoint.
    /// Usage:
    /// [RequiresPermission("MODELSWMS.MASTERS.LOCATIONS.ACCESS")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class RequiresPermissionAttribute : TypeFilterAttribute
    {
        public RequiresPermissionAttribute(string permissionKey)
            : base(typeof(RequiresPermissionFilter))
        {
            Arguments = new object[] { permissionKey };
        }
    }
}
