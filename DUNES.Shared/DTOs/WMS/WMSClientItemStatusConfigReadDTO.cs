using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Read DTO for Client Item Status configuration row.
    /// </summary>
    public class WMSClientItemStatusConfigReadDTO
    {
        /// <summary>Mapping identity.</summary>
        public int Id { get; set; }

        /// <summary>CompaniesContract id (client profile id).</summary>
        public int CompaniesContractId { get; set; }

        /// <summary>Master ItemStatus id.</summary>
        public int ItemStatusId { get; set; }

        /// <summary>Mapping active flag.</summary>
        public bool IsActive { get; set; }

        /// <summary>Master name (optional for UI).</summary>
        public string? ItemStatusName { get; set; }

        /// <summary>Master active flag (for UI validation).</summary>
        public bool MasterActive { get; set; }
    }
}
