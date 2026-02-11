using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// DTO used to assign a new item status to a client.
    /// </summary>
    public class WMSCompanyClientItemStatusCreateDTO
    {
        /// <summary>Master Item Status ID to assign.</summary>
        public int ItemStatusId { get; set; }
    }
}
