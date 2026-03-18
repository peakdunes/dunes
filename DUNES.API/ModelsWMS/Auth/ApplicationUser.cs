using Microsoft.AspNetCore.Identity;

namespace DUNES.API.ModelsWMS.Auth
{
    /// <summary>
    /// Represents the application user for DUNES.
    /// Extends the default IdentityUser with internal business fields
    /// required for user lifecycle management and security control.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Full display name of the user.
        /// This value is used in the UI and reports instead of the login username.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the user account is active.
        /// If false, the user is not allowed to sign in even with valid credentials.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Indicates whether the user must change the password on next login.
        /// This is normally true for newly created users and after an administrative password reset.
        /// </summary>
        public bool MustChangePassword { get; set; } = true;

        /// <summary>
        /// UTC date and time when the user record was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// User identifier of the person who created this record.
        /// Can be null when the value is not available.
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// UTC date and time of the last update applied to this user.
        /// Can be null if the record has not been modified after creation.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// User identifier of the person who last updated this record.
        /// Can be null when the value is not available.
        /// </summary>
        public string? UpdatedBy { get; set; }
    }
}
