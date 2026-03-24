namespace DUNES.UI.Models.Auth
{
    /// <summary>
    /// UI model for a permission checkbox item assigned to a role.
    /// </summary>
    public class RolePermissionSelectionItemVM
    {
        /// <summary>
        /// Permission identifier.
        /// </summary>
        public int PermissionId { get; set; }

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
        /// Indicates whether the permission is selected.
        /// </summary>
        public bool Assigned { get; set; }
    }
}