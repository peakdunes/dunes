using Microsoft.AspNetCore.Mvc.Rendering;

namespace DUNES.UI.Models.Auth
{
    public class RolePermissionsMatrixPageVM
    {
        public string RoleId { get; set; } = string.Empty;
        public List<SelectListItem> Roles { get; set; } = new();

        public List<string> Actions { get; set; } = new();
        public List<RolePermissionsMatrixRowVM> Rows { get; set; } = new();
    }
}
