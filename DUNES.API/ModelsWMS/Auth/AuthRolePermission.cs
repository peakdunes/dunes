using System.ComponentModel.DataAnnotations;

namespace DUNES.API.ModelsWMS.Auth
{
    /// <summary>
    /// Role -> Permission (many-to-many)
    /// </summary>
    public class AuthRolePermission
    {
        /// <summary>
        /// Role Id
        /// </summary>
        [MaxLength(450)]
        public string RoleId { get; set; } = string.Empty;

        /// <summary>
        /// Permission Id
        /// </summary>
        public int PermissionId { get; set; }
    }
}
