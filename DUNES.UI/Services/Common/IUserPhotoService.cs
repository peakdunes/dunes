namespace DUNES.UI.Services.Common
{
    public interface IUserPhotoService
    {
        string GetPhotoUrl(string? userId);
        string GetDefaultPhotoUrl();
    }
}
