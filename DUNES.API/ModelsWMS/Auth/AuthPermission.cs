using System.ComponentModel.DataAnnotations;

namespace DUNES.API.ModelsWMS.Auth
{
    /// <summary>
    /// Catalog of permissions (stable keys like: WMS.LOCATIONS.ACCESS).
    /// </summary>
    public class AuthPermission
    {

        /// <summary>
        /// database id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Permission Key
        /// </summary>
        [MaxLength(150)]
        public string PermissionKey { get; set; } = string.Empty;

        /// <summary>
        /// Description
        /// </summary>
        [MaxLength(300)]
        public string? Description { get; set; }

        /// <summary>
        /// Is Active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Date Created
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
