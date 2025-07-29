namespace APIZEBRA.DTOs.Auth
{
    /// <summary>
    /// Represents a menu item returned by the API for authenticated users.
    /// It supports up to 5 hierarchical levels and nested children.
    /// </summary>
    public class MenuItemDto
    {
        /// <summary>
        /// Unique code for the menu (used to determine the hierarchy).
        /// Example: "01", "0101", "010101"
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The display name of the menu item.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A short description of the menu functionality.
        /// </summary>
        public string? Utility { get; set; }

        /// <summary>
        /// Controller name for navigation.
        /// </summary>
        public string? Controller { get; set; }

        /// <summary>
        /// Action name for navigation.
        /// </summary>
        public string? Action { get; set; }

        /// <summary>
        /// Comma-separated list of roles that can access this menu.
        /// Example: "Admin,Basic"
        /// </summary>
        public string? Roles { get; set; }

        /// <summary>
        /// Indicates whether the menu item is active (true) or disabled (false).
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Order in which this menu should appear.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Nested children of this menu item.
        /// </summary>
        public List<MenuItemDto> Children { get; set; } = new();
    }
}
