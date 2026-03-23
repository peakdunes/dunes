using System.ComponentModel.DataAnnotations;

namespace DUNES.API.ModelsWMS.Auth
{
    /// <summary>
    /// Catalog of permissions (stable keys like: WMS.LOCATIONS.ACCESS).
    /// </summary>
    public class AuthPermission
    {

        /// <summary>
        /// Database Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Unique technical permission key.
        /// Example: Locations.Create
        /// </summary>
        [MaxLength(150)]
        public string PermissionKey { get; set; } = string.Empty;

        /// <summary>
        /// Functional group shown in UI.
        /// Example: Masters, Security, Operations, Reports
        /// </summary>
        [MaxLength(100)]
        public string GroupName { get; set; } = string.Empty;

        /// <summary>
        /// Functional module affected by this permission.
        /// Example: Locations, Transaction Concepts, Users
        /// </summary>
        [MaxLength(100)]
        public string ModuleName { get; set; } = string.Empty;

        /// <summary>
        /// Action label shown in UI.
        /// Example: Access, Create, Update, Delete, Approve, Export
        /// </summary>
        [MaxLength(100)]
        public string ActionName { get; set; } = string.Empty;

        /// <summary>
        /// Functional description of what the permission allows.
        /// </summary>
        [MaxLength(300)]
        public string? Description { get; set; }

        /// <summary>
        /// Order used to display the permission within the module.
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Indicates whether the permission is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Date created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
