using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Update DTO for Item Status (WMS).
    /// STANDARD COMPANYID: CompanyId is NOT accepted from client.
    /// </summary>
    public class WMSItemStatusUpdateDTO
    {
        /// <summary>
        /// Database identifier.
        /// IMPORTANT: Controller forces this from route id (route is authoritative).
        /// </summary>
        public int Id { get; set; }

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
