namespace DUNES.API.Services.Auth
{
    /// <summary>
    /// User Identification 
    /// </summary>
    public interface ICurrentUser
    {
        /// <summary>
        /// User id
        /// </summary>
        string? UserId { get; }

        /// <summary>
        /// User Name
        /// </summary>
        string? UserName { get; }

        /// <summary>
        /// User Role List
        /// </summary>
        IEnumerable<string> Roles { get; }

        /// <summary>
        /// User Is authenticated (Y/N)
        /// </summary>
        bool IsAuthenticated { get; }
    }
}
