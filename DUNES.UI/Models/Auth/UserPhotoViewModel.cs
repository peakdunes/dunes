namespace DUNES.UI.Models.Auth
{
    public class UserPhotoViewModel
    {
        public string? UserId { get; set; }

        // thumb | card | navbar
        public string Variant { get; set; } = "thumb";

        public string AltText { get; set; } = "User Photo";

        public string AdditionalCssClass { get; set; } = string.Empty;
    }
}
