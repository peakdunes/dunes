using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Auth
{
    /// <summary>
    /// Request DTO to save the full permission set for a role.
    /// </summary>
    public class SaveRolePermissionsDTO
    {
        /// <summary>
        /// Role identifier.
        /// </summary>
        public string RoleId { get; set; } = string.Empty;

        /// <summary>
        /// Selected permission identifiers for the role.
        /// </summary>
        public List<int> PermissionIds { get; set; } = new();
    }
}