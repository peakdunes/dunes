using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Auth
{
    namespace DUNES.Shared.DTOs.Auth
    {
        /// <summary>
        /// Response DTO containing inherited, direct, and effective permissions for a user.
        /// </summary>
        public class UserPermissionBundleDTO
        {
            /// <summary>
            /// User identifier.
            /// </summary>
            public string UserId { get; set; } = string.Empty;

            /// <summary>
            /// Permissions inherited from the user's roles.
            /// </summary>
            public List<RolePermissionItemDTO> InheritedPermissions { get; set; } = new();

            /// <summary>
            /// Direct permissions assigned specifically to the user.
            /// </summary>
            public List<RolePermissionItemDTO> DirectPermissions { get; set; } = new();

            /// <summary>
            /// Effective permissions resulting from inherited + direct permissions.
            /// </summary>
            public List<RolePermissionItemDTO> EffectivePermissions { get; set; } = new();

            public List<RolePermissionItemDTO> AvailableDirectPermissions { get; set; } = new();
        }
    }
}
