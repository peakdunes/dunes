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
        /// <summary>Mapping ID.</summary>
        public int Id { get; set; }

        /// <summary>Master InventoryType ID.</summary>
        public int InventoryTypeId { get; set; }

        /// <summary>Type name (from master catalog).</summary>
        public string InventoryTypeName { get; set; } = string.Empty;

        /// <summary>Mapping-level enable flag.</summary>
        public bool IsActive { get; set; }
    }
}
