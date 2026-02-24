using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Read DTO for client-level inventory type mapping.
    /// </summary>
    public class WMSCompanyClientInventoryTypeReadDTO
    {
        /// Mapping record Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// FK to master InventoryTypes catalog.
        /// </summary>
        public int InventoryTypeId { get; set; }

        /// <summary>
        /// InventoryType display name from master catalog.
        /// </summary>
        public string InventoryTypeName { get; set; } = string.Empty;

        /// <summary>
        /// Mapping-level active flag.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
