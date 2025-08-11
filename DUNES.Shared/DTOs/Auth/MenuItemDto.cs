using System.Text.Json.Serialization;

namespace DUNES.Shared.DTOs.Auth
{
    /// <summary>
    /// Represents a single menu item in the application, including hierarchy and access control.
    /// </summary>
    public class MenuItemDto
    {
        /// <summary>
        /// Unique code for the menu item. 
        /// Level1 = 2 chars, Level2 = 4 chars, etc.
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Title to display on the menu.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Short description of the menu option.
        /// </summary>
        [JsonPropertyName("utility")]
        public string Utility { get; set; } = string.Empty;

        /// <summary>
        /// MVC Controller linked to this menu item.
        /// </summary>
        [JsonPropertyName("controller")]
        public string Controller { get; set; } = string.Empty;

        /// <summary>
        /// MVC Action linked to this menu item.
        /// </summary>
        [JsonPropertyName("action")]
        public string Action { get; set; } = string.Empty;

        /// <summary>
        /// Roles that have access to this menu (comma-separated).
        /// </summary>
        [JsonPropertyName("roles")]
        public string Roles { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if the menu item is active.
        /// </summary>
        [JsonPropertyName("active")]
        public bool Active { get; set; }

        /// <summary>
        /// Order of the menu item for sorting.
        /// </summary>
        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("previousmenu")]
        public string previousmenu { get;set; }
        ///// <summary>
        ///// Children menu items (submenus).
        ///// </summary>
        //public List<MenuItemDto> Children { get; set; } = new();
    }
}
