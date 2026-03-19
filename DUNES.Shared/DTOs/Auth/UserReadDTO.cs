using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Auth
{
    public class UserReadDTO
    {
        public string Id { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public bool MustChangePassword { get; set; }

        public string? RoleId { get; set; }

        public string? RoleName { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
