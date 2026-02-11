using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Create DTO for mapping an inventory category to a client.
    /// </summary>
    public class WMSCompanyClientInventoryCategoryCreateDTO
    {
        /// <summary>
        /// Inventory category master ID to assign to the client.
        /// </summary>
        [Required]
        public int InventoryCategoryId { get; set; }

        /// <summary>
        /// Optional initial active flag (default true).
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
