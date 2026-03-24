namespace DUNES.UI.Models.Auth
{
    public class SaveRolePermissionsMatrixVM
    {
        public string RoleId { get; set; } = string.Empty;
        public List<int> PermissionIds { get; set; } = new();
    }
}
