using Microsoft.AspNetCore.Mvc.Rendering;

namespace DUNES.UI.Models.Auth
{
    public class UserPermissionsPageVM
    {
        public string UserId { get; set; } = string.Empty;
        public List<SelectListItem> Users { get; set; } = new();

        public List<string> Actions { get; set; } = new();

        public List<RolePermissionsMatrixRowVM> InheritedRows { get; set; } = new();
        public List<RolePermissionsMatrixRowVM> AdditionalRows { get; set; } = new();
        public List<RolePermissionsMatrixRowVM> EffectiveRows { get; set; } = new();
    }
}
