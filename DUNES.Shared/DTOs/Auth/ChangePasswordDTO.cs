using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Auth
{
    /// <summary>
    /// DTO used to change the password of the current authenticated user.
    /// </summary>
    public class ChangePasswordDTO
    {
        /// <summary>
        /// Current password of the user.
        /// </summary>
        public string CurrentPassword { get; set; } = string.Empty;

        /// <summary>
        /// New password to assign.
        /// </summary>
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// Confirmation of the new password.
        /// </summary>
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
