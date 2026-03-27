using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Auth
{
    /// <summary>
    /// Stores the authenticated user's effective permissions in session.
    /// </summary>
    public class UserPermissionSessionDTO
    {
        /// <summary>
        /// Authenticated user id.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Effective permissions for the authenticated user.
        /// </summary>
        public HashSet<string> Permissions { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    }
}
