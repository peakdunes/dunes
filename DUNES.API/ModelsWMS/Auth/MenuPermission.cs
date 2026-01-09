namespace DUNES.API.ModelsWMS.Auth
{
    /// <summary>
    /// Menu -> Permission mapping (usually ACCESS permission).
    /// </summary>
    public class MenuPermission
    {
        /// <summary>
        /// Menu Id
        /// </summary>
        public int MenuId { get; set; }
        /// <summary>
        /// Permission Id
        /// </summary>
        public int PermissionId { get; set; }
    }
}
