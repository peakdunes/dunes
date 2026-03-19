namespace DUNES.UI.Models
{
    public class CrudActionsModel
    {
        //public int Id { get; set; }
        public string RouteId { get; set; } = string.Empty;
        // If null, it uses current controller
        public string? Controller { get; set; }

        public string EditAction { get; set; } = "Edit";
        public string DeleteAction { get; set; } = "Delete";
        public string ResetAction { get; set; } = "ResetPassword";
        public string DeactivateAction { get; set; } = "Deactivate";

        // Para cambiar título o texto si mañana lo necesitas
        public string ResetTitle { get; set; } = "Reset Password";
        public string DeactivateTitle { get; set; } = "Deactivate";

        public bool ShowEdit { get; set; } = true;
        public bool ShowDelete { get; set; } = true;
        public bool ShowReset { get; set; } = false;
        public bool ShowDeactivate { get; set; } = false;
        public bool IsActive { get; set; } = true;

    }
}
