using DUNES.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DUNES.UI.Models
{
    /// <summary>
    /// UI model for the Role Permissions page.
    /// </summary>
    public class RolePermissionsPageVM
    {
        public string? RoleId { get; set; }
        public List<SelectListItem> Roles { get; set; } = new();
        public List<RolePermissionItemVM> AssignedPermissions { get; set; } = new();
        public List<RolePermissionItemVM> AvailablePermissions { get; set; } = new();
    }
}