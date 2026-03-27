using System.ComponentModel.DataAnnotations;

namespace DUNES.API.ModelsWMS.Auth
{
    /// <summary>
    /// Catalog of permissions with optional UI metadata for MVC rendering.
    /// </summary>
    public class AuthPermission
    {
        /// <summary>
        /// Database Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Unique technical permission key.
        /// Example: Masters.Locations.Access
        /// </summary>
        [MaxLength(150)]
        public string PermissionKey { get; set; } = string.Empty;

        /// <summary>
        /// Functional group shown in UI.
        /// Example: Masters, Security, Operations, Reports.
        /// </summary>
        [MaxLength(100)]
        public string GroupName { get; set; } = string.Empty;

        /// <summary>
        /// Functional module affected by this permission.
        /// Example: Locations, Transaction Concepts, Users.
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
        /// Order used to display the permission within the module.
        /// Also used as default UI sort order.
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Indicates whether the permission is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Indicates whether this permission should be rendered as a row action button.
        /// Example: Edit, Delete, ResetPassword, Deactivate.
        /// </summary>
        public bool ShowAsRowAction { get; set; } = false;

        /// <summary>
        /// Indicates whether this permission should be rendered as a toolbar/header action.
        /// Example: Create, Export.
        /// </summary>
        public bool ShowAsToolbarAction { get; set; } = false;

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
        public int ButtonOrder { get; set; } = 0;

        /// <summary>
        /// Indicates whether the UI should ask for confirmation before executing the action.
        /// Useful for Delete, Deactivate, or other destructive actions.
        /// </summary>
        public bool RequiresConfirmation { get; set; } = false;

        /// <summary>
        /// Confirmation message shown before executing the action.
        /// Example: Are you sure you want to delete this record?
        /// </summary>
        [MaxLength(250)]
        public string? ConfirmationMessage { get; set; }

        /// <summary>
        /// Date created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}