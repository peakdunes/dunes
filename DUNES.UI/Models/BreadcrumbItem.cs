namespace DUNES.UI.Models
{

    /// <summary>
    /// Navegation Menu
    /// </summary>
    ///          
    public class BreadcrumbItem
    {
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Si Url es null, se considera el item actual (activo).
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// True cuando es la página actual (no tiene URL).
        /// </summary>
        public bool IsActive => string.IsNullOrWhiteSpace(Url);
    }
}
