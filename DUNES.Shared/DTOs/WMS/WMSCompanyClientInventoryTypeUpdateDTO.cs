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
        /// <summary>
        /// Mapping record Id (surrogate key).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// FK to master InventoryTypes catalog.
        /// </summary>
        public int InventoryTypeId { get; set; }

        /// <summary>
        /// Mapping-level active flag.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
