using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.WiewModels.Auth
{
    /// <summary>
    /// View model used by the UI to change the password.
    /// </summary>
    public class ChangePasswordVM
    {
        /// <summary>
        /// Current password of the user.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;

        /// <summary>
        /// New password to assign.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// Confirmation of the new password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}