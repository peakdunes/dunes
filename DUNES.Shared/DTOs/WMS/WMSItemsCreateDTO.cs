using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    public class WMSItemsCreateDTO
    {
        /// <summary>
        /// Optional client owner when the item belongs to a client.
        /// Null means master/company item.
        /// </summary>
        [Display(Name = "Company Client")]
        public int? CompanyClientId { get; set; }

        /// <summary>
        /// Inventory category.
        /// </summary>
        [Required]
        [Display(Name = "Inventory Category")]
        public int InventoryCategoryId { get; set; }

        /// <summary>
        /// Part Number. Required and unique.
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Display(Name = "Part Number")]
        public string PartNumber { get; set; } = string.Empty;

        /// <summary>
        /// Optional SKU.
        /// </summary>
        [MaxLength(50)]
        [Display(Name = "SKU")]
        public string? Sku { get; set; }

        /// <summary>
        /// Item description.
        /// </summary>
        [Required]
        [MaxLength(500)]
        [Display(Name = "Item Description")]
        public string ItemDescription { get; set; } = string.Empty;

        /// <summary>
        /// Optional barcode.
        /// </summary>
        [MaxLength(50)]
        [Display(Name = "Barcode")]
        public string? Barcode { get; set; }

        [Display(Name = "Is Repairable")]
        public bool IsRepairable { get; set; }

        [Display(Name = "Is Serialized")]
        public bool IsSerialized { get; set; }

        [Display(Name = "Is Active")]
        public bool Active { get; set; } = true;
    }
}
