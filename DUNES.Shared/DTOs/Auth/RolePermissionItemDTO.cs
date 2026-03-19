using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Auth
{
    /// <summary>
    /// Permission item assigned or available for a role.
    /// </summary>
    public class RolePermissionItemDTO
    {
        /// <summary>
        /// Permission identifier.
        /// </summary>
        public int PermissionId { get; set; }

        /// <summary>
        /// Technical permission key.
        /// </summary>
        public string PermissionKey { get; set; } = string.Empty;

        /// <summary>
        /// Functional group.
        /// </summary>
        public string Group { get; set; } = string.Empty;

        /// <summary>
        /// Resource name.
        /// </summary>
        public string Resource { get; set; } = string.Empty;

        /// <summary>
        /// Action name.
        /// </summary>
        public string Action { get; set; } = string.Empty;

        /// <summary>
        /// Permission description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Indicates whether the role currently has this permission assigned.
        /// </summary>
        public bool Assigned { get; set; }

        /// <summary>
        /// Indicates whether the permission is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}