using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Create DTO for Item Status (WMS).
    /// STANDARD COMPANYID: CompanyId is NOT accepted from client.
    /// </summary>
    public class WMSItemStatusCreateDTO
    {
        /// <summary>
        /// Status name.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        /// <summary>
        /// Observations (max 1000 chars).
        /// </summary>
        [MaxLength(1000)]
        public string? Observations { get; set; }
    }
}
