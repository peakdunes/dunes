using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Read DTO for Client Inventory Type configuration row.
    /// </summary>
    public class WMSClientInventoryTypeConfigReadDTO
    {
        /// <summary>Mapping identity.</summary>
        public int Id { get; set; }

        /// <summary>CompaniesContract id (client profile id).</summary>
        public int CompaniesContractId { get; set; }

        /// <summary>Master InventoryType id.</summary>
        public int InventoryTypeId { get; set; }

        /// <summary>Mapping active flag.</summary>
        public bool IsActive { get; set; }

        /// <summary>Master name (optional for UI).</summary>
        public string? InventoryTypeName { get; set; }

        /// <summary>Master active flag (for UI validation).</summary>
        public bool MasterActive { get; set; }
    }
}