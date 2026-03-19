using DUNES.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DUNES.UI.Models
{
    /// <summary>
    /// UI model for the Role Permissions page.
    /// </summary>
    public class RolePermissionsPageVM
    {
        /// <summary>
        /// Selected role identifier.
        /// </summary>
        public string RoleId { get; set; } = string.Empty;

        /// <summary>
        /// Available roles for selection.
        /// </summary>
        public List<SelectListItem> Roles { get; set; } = new();

        /// <summary>
        /// Permission checkbox items.
        /// </summary>
        public List<RolePermissionSelectionItemVM> Permissions { get; set; } = new();
    }
}