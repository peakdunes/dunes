namespace DUNES.UI.Models
{
    public class RolePermissionItemVM
    {
        public int PermissionId { get; set; }
        public string Group { get; set; } = string.Empty;
        public string Resource { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
