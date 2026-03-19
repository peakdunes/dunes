namespace DUNES.UI.Models
{
    /// <summary>
    /// fotos de cada usuario si no existe usa default.jpg
    /// </summary>
    public class UserPhotoSettings
    {
        public string BaseUrl { get; set; } = "/uploads/users";
        public string DefaultFileName { get; set; } = "default.jpg";
        public string Extension { get; set; } = ".jpg";
    }
}
