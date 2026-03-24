using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Auth
{
    /// <summary>
    /// Request DTO to save direct permissions for a user.
    /// </summary>
    public class SaveUserPermissionsDTO
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Direct permission identifiers to assign to the user.
        /// </summary>
        public List<int> PermissionIds { get; set; } = new();
    }
}
