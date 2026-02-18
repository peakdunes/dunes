using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Create DTO for enabling a master ItemStatus for the current client.
    /// CompanyId and CompanyClientId are always resolved from the token.
    /// </summary>
    public class WMSCompanyClientItemStatusCreateDTO
    {
        /// <summary>Master ItemStatus ID to enable.</summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int ItemStatusId { get; set; }

        /// <summary>Mapping-level enable flag (default true).</summary>
        public bool IsActive { get; set; } = true;
    }
}
