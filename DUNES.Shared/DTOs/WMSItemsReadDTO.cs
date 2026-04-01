using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs
{
    public class WMSItemsReadDTO
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public int? CompanyClientId { get; set; }

        public int InventoryCategoryId { get; set; }

        public string InventoryCategoryName { get; set; } = string.Empty;

        public string PartNumber { get; set; } = string.Empty;

        public string? Sku { get; set; }

        public string ItemDescription { get; set; } = string.Empty;

        public string? Barcode { get; set; }

        public bool IsRepairable { get; set; }

        public bool IsSerialized { get; set; }

        public bool Active { get; set; }

        /// <summary>
        /// True when the item belongs to a client; false when it is a master/company item.
        /// </summary>
        public bool IsClientOwned => CompanyClientId.HasValue;

        /// <summary>
        /// Helper label for UI/reporting.
        /// </summary>
        public string OwnershipType => CompanyClientId.HasValue ? "Client" : "Master";
    }
}
