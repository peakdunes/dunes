namespace DUNES.UI.Models.Auth
{
    public class PermissionMatrixRowVM
    {
        public string Group { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;

        public Dictionary<string, PermissionMatrixCellVM> Cells { get; set; } = new();
    }
}
