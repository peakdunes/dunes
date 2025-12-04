namespace DUNES.UI.Models
{

    /// <summary>
    /// Navegation Menu
    /// </summary>
    public class BreadcrumbItem
    {
        public string Text { get; set; } = null!;
        public string? Url { get; set; }   // null = es la página actual (amarillo)
        public bool IsCurrent => string.IsNullOrEmpty(Url);
    }
}
