namespace DUNES.UI.Models
{
    public class CrudActionsModel
    {
        public int Id { get; set; }

        // If null, it uses current controller
        public string? Controller { get; set; }

        public string EditAction { get; set; } = "Edit";
        public string DeleteAction { get; set; } = "Delete";

        public bool ShowEdit { get; set; } = true;
        public bool ShowDelete { get; set; } = true;

       
    }
}
