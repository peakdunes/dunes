using System.ComponentModel.DataAnnotations;

namespace DUNES.API.ModelsWMS.Auth
{
    /// <summary>
    /// User -> Permission grants (exceptions without creating new roles).
    /// (No deny logic for now; just grants.)
    /// </summary>
    public class AuthUserPermission
    {
        /// <summary>
        /// User Id
        /// </summary>
        [MaxLength(450)]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Permission Id
        /// </summary>
        public int PermissionId { get; set; }
    }
}
