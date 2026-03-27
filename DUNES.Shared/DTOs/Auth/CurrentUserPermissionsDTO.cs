using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Auth
{
    public class CurrentUserPermissionsDTO
    {
        public string UserId { get; set; } = string.Empty;
        public List<string> Permissions { get; set; } = new();
    }
}
