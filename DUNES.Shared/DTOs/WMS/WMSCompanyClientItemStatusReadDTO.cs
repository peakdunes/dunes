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

        /// <summary>Client contract ID (CompanyClientId).</summary>
        public int CompaniesContractId { get; set; }

        /// <summary>Master Item Status ID.</summary>
        public int ItemStatusId { get; set; }

        /// <summary>Item Status Name (readable).</summary>
        public string ItemStatusName { get; set; } = string.Empty;

        /// <summary>Indicates if the mapping is currently active.</summary>
        public bool IsActive { get; set; }
    }
}
