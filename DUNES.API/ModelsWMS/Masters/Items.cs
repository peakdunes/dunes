using System.ComponentModel.DataAnnotations;

namespace DUNES.API.ModelsWMS.Masters
{
    /// <summary>
    /// items
    /// </summary>
    public class Items
    {
        /// <summary>
        /// Internal identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Company owner scope.
        /// </summary>
        [Required]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        /// <summary>
        /// Client company when this item belongs to a specific client.
        /// Null means this is a company/master item.
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
        /// Part Number.
        /// Business rule: required and unique.
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Display(Name = "Part Number")]
        public string PartNumber { get; set; } = string.Empty;

        /// <summary>
        /// SKU (Stock Keeping Unit).
        /// Optional internal/commercial code.
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
        /// Barcode.
        /// Optional. Not used as the duplicate validation key.
        /// </summary>
        [MaxLength(50)]
        [Display(Name = "Barcode")]
        public string? Barcode { get; set; }

        /// <summary>
        /// Indicates whether the item is repairable.
        /// </summary>
        [Display(Name = "Is Repairable")]
        public bool IsRepairable { get; set; }

        /// <summary>
        /// Indicates whether the item is serialized.
        /// </summary>
        [Display(Name = "Is Serialized")]
        public bool IsSerialized { get; set; }

        /// <summary>
        /// Indicates whether the item is active.
        /// </summary>
        [Display(Name = "Is Active")]
        public bool Active { get; set; }

        /// <summary>
        /// Navigation to company.
        /// </summary>
        public virtual Company Company { get; set; } = null!;

        /// <summary>
        /// Navigation to company client (optional).
        /// </summary>
        public virtual CompanyClient? CompanyClient { get; set; }

        /// <summary>
        /// Navigation to inventory category.
        /// </summary>
        public virtual Inventorycategories InventoryCategory { get; set; } = null!;
    }
}
