using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Update DTO for client-inventory type mapping.
    /// </summary>
    public class WMSCompanyClientInventoryTypeUpdateDTO
    {
        /// <summary>Mapping ID.</summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        /// <summary>Mapping-level enable flag.</summary>
        public bool IsActive { get; set; } = true;
    }
}
