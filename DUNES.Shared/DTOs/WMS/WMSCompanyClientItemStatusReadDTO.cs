using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Read DTO for item status mappings assigned to a client.
    /// </summary>
    public class WMSCompanyClientItemStatusReadDTO
    {
        /// <summary>Mapping ID.</summary>
        public int Id { get; set; }

        /// <summary>Master ItemStatus ID.</summary>
        public int ItemStatusId { get; set; }

        /// <summary>Status name (from master catalog).</summary>
        public string ItemStatusName { get; set; } = string.Empty;

        /// <summary>Mapping-level enable flag.</summary>
        public bool IsActive { get; set; }
    }
}
