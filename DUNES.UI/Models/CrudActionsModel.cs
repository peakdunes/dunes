namespace DUNES.UI.Models
{
    public class CrudActionsModel
    {
        public string? RouteId { get; set; }
        public string? Controller { get; set; }
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
    }
}