namespace DUNES.API.RepositoriesWMS.Auth
{
    /// <summary>
    /// Auth permissions repository (effective permissions for user).
    /// </summary>
    public interface IAuthPermissionRepository
    {
        /// <summary>
        /// Returns the effective permission keys for the user:
        /// Role permissions + User grants (only grants for now).
        /// </summary>
        Task<List<string>> GetEffectivePermissionKeysAsync(string userId, CancellationToken ct);
    }
}
