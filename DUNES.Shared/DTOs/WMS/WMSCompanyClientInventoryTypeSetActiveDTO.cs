using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    public class WMSCompanyClientInventoryTypeSetActiveDTO
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
