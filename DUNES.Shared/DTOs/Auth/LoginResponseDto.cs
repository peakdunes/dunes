using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Auth
{
    /// <summary>
    /// Represents the response returned after a successful login.
    /// Includes the JWT token, expiration date, user identity,
    /// active environment context, and password change requirement flag.
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// JWT token generated for the authenticated user.
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Token expiration date and time in UTC.
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// Username used to authenticate into the system.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Default company identifier from the active user configuration.
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Default company name from the active user configuration.
        /// </summary>
        public string? companyName { get; set; }

        /// <summary>
        /// Default company client identifier from the active user configuration.
        /// </summary>
        public int CompanyClientId { get; set; }

        /// <summary>
        /// Default company client name from the active user configuration.
        /// </summary>
        public string? companyClientName { get; set; }

        /// <summary>
        /// Default location identifier from the active user configuration.
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        /// Default location name from the active user configuration.
        /// </summary>
        public string? LocationName { get; set; }

        /// <summary>
        /// Active environment name assigned to the user configuration.
        /// </summary>
        public string? Enviromentname { get; set; }

        /// <summary>
        /// Current role name associated with the active user configuration.
        /// </summary>
        public string? RoleName { get; set; }

        /// <summary>
        /// Default contract identifier from the active user configuration.
        /// </summary>
        public int companiesContractId { get; set; }

        /// <summary>
        /// Indicates whether the user must change the password
        /// before continuing to use the application.
        /// </summary>
        public bool MustChangePassword { get; set; }


        /// <summary>
        /// the user has a active configuration? (userconfiguration table)
        /// </summary>
        public bool HasConfiguration { get; set; }
    }
}
