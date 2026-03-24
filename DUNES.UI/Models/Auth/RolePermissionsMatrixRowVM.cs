namespace DUNES.UI.Models.Auth
{
    public class RolePermissionsMatrixRowVM
    {
        public string Group { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;

        public Dictionary<string, RolePermissionsMatrixCellVM> Cells { get; set; } = new();
    }
}
