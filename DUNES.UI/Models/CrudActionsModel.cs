using System.ComponentModel.DataAnnotations;

namespace DUNES.UI.Models
{
    public class CrudActionsModel
    {
        public string? Controller { get; set; }

        /// <summary>
        /// Source values available to resolve RouteParamsTemplate placeholders.
        /// Examples:
        /// { "Id" : 10 }
        /// { "CompanyId" : 5, "CompanyClientId" : 9 }
        /// { "Code" : "WH1" }
        /// </summary>
        public Dictionary<string, object?> RouteSourceValues { get; set; } = new();

        public List<CrudActionItemModel> Actions { get; set; } = new();
    }

    public class CrudActionItemModel
    {
        public string PermissionKey { get; set; } = string.Empty;
        public string MvcActionName { get; set; } = string.Empty;
        public string? ButtonText { get; set; }
        public string? IconCss { get; set; }
        public string? ButtonCss { get; set; }
        public string? TextCss { get; set; }
        public int ButtonOrder { get; set; }
        public bool RequiresConfirmation { get; set; }
        public string? ConfirmationMessage { get; set; }
        public string? RouteParamsTemplate { get; set; }
    }
}