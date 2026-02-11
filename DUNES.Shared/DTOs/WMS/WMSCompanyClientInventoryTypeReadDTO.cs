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
        /// <summary>
        /// Mapping identity.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Inventory type master ID.
        /// </summary>
        public int InventoryTypeId { get; set; }

        /// <summary>
        /// Inventory type name (from master).
        /// </summary>
        public string InventoryTypeName { get; set; } = string.Empty;

        /// <summary>
        /// Active flag for this client mapping.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
