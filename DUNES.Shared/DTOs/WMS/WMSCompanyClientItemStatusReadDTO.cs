using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Read DTO for ItemStatus mappings assigned to a client.
    /// </summary>
    public class WMSCompanyClientItemStatusReadDTO
    {
        /// <summary>
        /// Mapping identifier (surrogate key).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// FK to master Itemstatus catalog.
        /// </summary>
        public int ItemStatusId { get; set; }

        /// <summary>
        /// Display name from master Itemstatus catalog.
        /// </summary>
        public string ItemStatusName { get; set; } = string.Empty;

        /// <summary>
        /// Mapping-level active flag.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
