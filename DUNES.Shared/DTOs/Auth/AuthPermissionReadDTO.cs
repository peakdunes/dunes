using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Auth
{
    /// <summary>
    /// Read DTO for permission catalog records, including optional UI metadata
    /// used to render MVC actions and buttons dynamically.
    /// </summary>
    public class AuthPermissionReadDTO
    {
        /// <summary>
        /// Database identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Unique technical permission key.
        /// Example: Masters.Locations.Update
        /// </summary>
        [MaxLength(150)]
        public string PermissionKey { get; set; } = string.Empty;

        /// <summary>
        /// Functional group shown in UI.
        /// Example: Masters, Auth, Reports.
        /// </summary>
        [MaxLength(100)]
        public string GroupName { get; set; } = string.Empty;

        /// <summary>
        /// Functional module affected by this permission.
        /// Example: Locations, Users, CompanyClientItemStatus.
        /// </summary>
        [MaxLength(100)]
        public string ModuleName { get; set; } = string.Empty;

        /// <summary>
        /// Logical action label of the permission.
        /// Example: Access, Create, Update, Delete, Approve, Export.
        /// </summary>
        [MaxLength(100)]
        public string ActionName { get; set; } = string.Empty;

        /// <summary>
        /// MVC action name used by the UI when rendering a button or link.
        /// Example: Index, Create, Edit, Delete, ResetPassword, Export.
        /// </summary>
        [MaxLength(100)]
        public string? MvcActionName { get; set; }

        /// <summary>
        /// Functional description of what the permission allows.
        /// </summary>
        [MaxLength(300)]
        public string? Description { get; set; }

        /// <summary>
        /// Indicates whether the permission is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Order used to display the permission within the module.
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Indicates whether this permission should be rendered as a row action button.
        /// </summary>
        public bool ShowAsRowAction { get; set; }

        /// <summary>
        /// Indicates whether this permission should be rendered as a toolbar or header action.
        /// </summary>
        public bool ShowAsToolbarAction { get; set; }

        /// <summary>
        /// Text shown in the UI for the button, link, or tooltip.
        /// Example: Edit, Delete, Reset Password, Export.
        /// </summary>
        [MaxLength(100)]
        public string? ButtonText { get; set; }

        /// <summary>
        /// Icon css class used in the UI.
        /// Example: fas fa-pencil-alt, fas fa-trash-alt, fas fa-key.
        /// </summary>
        [MaxLength(100)]
        public string? IconCss { get; set; }

        /// <summary>
        /// CSS classes used to style the button.
        /// Example: btn btn-outline-primary.
        /// </summary>
        [MaxLength(150)]
        public string? ButtonCss { get; set; }

        /// <summary>
        /// Optional text css classes used together with the button.
        /// Example: text-primary, text-danger.
        /// </summary>
        [MaxLength(100)]
        public string? TextCss { get; set; }

        /// <summary>
        /// Sort order for button rendering in the UI.
        /// </summary>
        public int ButtonOrder { get; set; }

        /// <summary>
        /// Indicates whether the UI should ask for confirmation before executing the action.
        /// </summary>
        public bool RequiresConfirmation { get; set; }

        /// <summary>
        /// Confirmation message shown before executing the action.
        /// Example: Are you sure you want to delete this record?
        /// </summary>
        [MaxLength(250)]
        public string? ConfirmationMessage { get; set; }


        /// <summary>
        /// Template used to build route values dynamically in the UI.
        /// Examples:
        /// id={Id}
        /// companyId={CompanyId}
        /// companyId={CompanyId}&companyClientId={CompanyClientId}
        /// code={Code}
        /// </summary>
        [MaxLength(500)]
        public string? RouteParamsTemplate { get; set; }

        /// <summary>
        /// Date created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
