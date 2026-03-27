using DUNES.Shared.DTOs.Auth;

namespace DUNES.UI.Helpers
{
    /// <summary>
    /// Provides access to the authenticated user's permission session data.
    /// </summary>
    public interface IUserPermissionSessionHelper
    {
        /// <summary>
        /// Saves the authenticated user's permissions in session.
        /// </summary>
        /// <param name="data">Permission session data.</param>
        void Save(UserPermissionSessionDTO data);

        /// <summary>
        /// Gets the authenticated user's permissions from session.
        /// </summary>
        /// <returns>Permission session data or null if not found.</returns>
        UserPermissionSessionDTO? Get();

        /// <summary>
        /// Determines whether the authenticated user has the specified permission.
        /// </summary>
        /// <param name="permission">Permission key.</param>
        /// <returns>True if the permission exists in session; otherwise false.</returns>
        bool HasPermission(string permission);

        /// <summary>
        /// Removes the authenticated user's permissions from session.
        /// </summary>
        void Clear();
    }
}
