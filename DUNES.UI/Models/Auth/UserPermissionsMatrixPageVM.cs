using DUNES.UI.Models.Auth;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace DUNES.UI.Models.Auth
{
    public class UserPermissionsMatrixPageVM
    {
        public string UserId { get; set; } = string.Empty;

        public List<SelectListItem> Users { get; set; } = new();

        public List<string> Actions { get; set; } = new();

        public List<PermissionMatrixRowVM> InheritedRows { get; set; } = new();
        public List<PermissionMatrixRowVM> DirectRows { get; set; } = new();
        public List<PermissionMatrixRowVM> EffectiveRows { get; set; } = new();
    }
}
