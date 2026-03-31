using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Auth
{
    public class AuthPermissionCreateDTO
    {
        [MaxLength(150)]
        public string PermissionKey { get; set; } = string.Empty;

        [MaxLength(100)]
        public string GroupName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string ModuleName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string ActionName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? MvcActionName { get; set; }

        [MaxLength(300)]
        public string? Description { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;

        public bool ShowAsRowAction { get; set; } = false;

        public bool ShowAsToolbarAction { get; set; } = false;

        [MaxLength(100)]
        public string? ButtonText { get; set; }

        [MaxLength(100)]
        public string? IconCss { get; set; }

        [MaxLength(150)]
        public string? ButtonCss { get; set; }

        [MaxLength(100)]
        public string? TextCss { get; set; }

        public int ButtonOrder { get; set; } = 0;

        public bool RequiresConfirmation { get; set; } = false;

        [MaxLength(250)]
        public string? ConfirmationMessage { get; set; }

        [MaxLength(500)]
        public string? RouteParamsTemplate { get; set; }
    }
}
