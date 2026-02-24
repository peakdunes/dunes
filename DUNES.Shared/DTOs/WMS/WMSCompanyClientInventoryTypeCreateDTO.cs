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
        /// <summary>
        /// FK to master InventoryTypes catalog.
        /// </summary>
        public int InventoryTypeId { get; set; }

        /// <summary>
        /// Mapping-level active flag (default true).
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
