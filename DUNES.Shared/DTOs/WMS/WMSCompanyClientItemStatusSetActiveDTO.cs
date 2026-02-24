using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Request DTO to activate or deactivate an existing ItemStatus mapping.
    /// </summary>
    public class WMSCompanyClientItemStatusSetActiveDTO
    {
        /// <summary>
        /// Mapping identifier (surrogate key).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// New active status for the mapping.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
