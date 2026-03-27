using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.Auth.Authorization
{
    /// <summary>
    /// Requires a specific permission key for the endpoint.
    /// Usage:
    /// [RequiresPermission("Masters.Locations.Access")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class RequiresPermissionAttribute : TypeFilterAttribute
    {

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="permissionKey"></param>
        public RequiresPermissionAttribute(string permissionKey)
            : base(typeof(RequiresPermissionFilter))
        {
            Arguments = new object[] { permissionKey };
        }
    }
}