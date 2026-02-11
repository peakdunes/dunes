using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// DTO used to update the active status of a client-item status mapping.
    /// </summary>
    public class WMSCompanyClientItemStatusUpdateDTO
    {
        /// <summary>Mapping ID to update.</summary>
        public int Id { get; set; }

        /// <summary>Item Status ID (FK).</summary>
        public int ItemStatusId { get; set; }

        /// <summary>New active state to apply.</summary>
        public bool IsActive { get; set; }
    }
}
