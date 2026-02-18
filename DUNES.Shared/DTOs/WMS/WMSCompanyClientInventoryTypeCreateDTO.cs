using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Create DTO for mapping an inventory type to a client.
    /// </summary>
    public class WMSCompanyClientInventoryTypeCreateDTO
    {
        /// <summary>Master InventoryType ID to enable.</summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int InventoryTypeId { get; set; }

        /// <summary>Mapping-level enable flag (default true).</summary>
        public bool IsActive { get; set; } = true;
    }
}
