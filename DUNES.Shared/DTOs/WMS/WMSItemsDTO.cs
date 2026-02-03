using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Item DTO
    /// Represents an inventory item owned by the company.
    /// CompanyClientId is not exposed because this DTO is scoped
    /// to company-owned items only.
    /// </summary>
    public class WMSItemsDTO
    {
        /// <summary>
        /// Internal database identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Company identifier (set from token, not trusted from client)
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Inventory category identifier
        /// </summary>
        public int InventorycategoriesId { get; set; }

        /// <summary>
        /// Stock Keeping Unit (SKU)
        /// Unique code used to identify the item
        /// </summary>
        public required string Sku { get; set; }

        /// <summary>
        /// Item description
        /// </summary>
        public required string ItemDescription { get; set; }

        /// <summary>
        /// Barcode value (optional)
        /// </summary>
        public string? Barcode { get; set; }

        /// <summary>
        /// Serial number (optional)
        /// </summary>
        public string? SerialNumber { get; set; }

        /// <summary>
        /// Indicates whether the item is repairable
        /// </summary>
        public bool IsRepairable { get; set; }

        /// <summary>
        /// Indicates whether the item is serialized
        /// </summary>
        public bool IsSerialized { get; set; }

        /// <summary>
        /// Indicates whether the item is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Inventory category name (read-only, for display purposes)
        /// </summary>
        public string? InventoryCategoryName { get; set; }
    }
}
