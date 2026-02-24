using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Request DTO to create a new ItemStatus mapping for a client.
    /// Tenant values (CompanyId, CompanyClientId) are always taken from token.
    /// </summary>
    public class WMSCompanyClientItemStatusCreateDTO
    {
        /// <summary>
        /// FK to master Itemstatus catalog.
        /// </summary>
        public int ItemStatusId { get; set; }

        /// <summary>
        /// Mapping-level active flag (default true).
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}