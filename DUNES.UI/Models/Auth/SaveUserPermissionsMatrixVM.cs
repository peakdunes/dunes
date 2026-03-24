using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Auth
{
    public class SaveUserPermissionsMatrixVM
    {
        public string UserId { get; set; } = string.Empty;
        public List<int> PermissionIds { get; set; } = new();
    }
}
