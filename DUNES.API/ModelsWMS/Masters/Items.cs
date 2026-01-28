using System.ComponentModel.DataAnnotations;

namespace DUNES.API.ModelsWMS.Masters
{
    /// <summary>
    /// items
    /// </summary>
    public class Items
    {
        /// <summary>
        /// internal id
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Company 
        /// </summary>
        [Required]
        [Display(Name = "Company")]
        public required int companyId { get; set; }


        /// <summary>
        /// client company when this item belong a client
        /// </summary>
        [Display(Name = "Company Client")]
        public int? CompanyClientId { get; set; }

        /// <summary>
        /// Company category
        /// </summary>
        [Display(Name = "Inventory Category")]
        public int InventorycategoriesId { get; set; }
                  


 /// <summary>
 ///  SKU (Stock Keeping Unit)
 /// </summary>
 [MaxLength(50)]
        [Display(Name ="SKU")]
        public required string sku { get; set; }

        /// <summary>
        /// item description
        /// </summary>
        [MaxLength(500)]
        [Display(Name = "Item Description")]
        public required string itemDescription { get; set; }

        /// <summary>
        /// Bar code
        /// </summary>
        [MaxLength(50)]
        [Display(Name = "Barcode")]
        public string? Barcode { get; set; }


        /// <summary>
        /// Serial Number
        /// </summary>
        [MaxLength(500)]
        [Display(Name = "Serial Number")]
        public string? serialnumber { get; set; }


        /// <summary>
        /// is Repairable
        /// </summary>
        /// 
        [Display(Name = "Is Repairable")]
        public bool isRepairable { get; set; }

        /// <summary>
        /// is serialized
        /// </summary>
        /// 
        [Display(Name = "Is serialized")]
        public bool isSerialized { get; set; }

        /// <summary>
        /// Is active
        /// </summary>
        /// 
        [Display(Name = "Is Active")]
        public bool active { get; set; }


        /// <summary>
        /// company navegation property
        /// </summary>
        public virtual Company IdcompanyNavigation { get; set; } = null!;


        /// <summary>
        /// Navigation to client company (optional)
        /// </summary>
        public virtual CompanyClient? CompanyClient { get; set; }

        /// <summary>
        /// Navegation inventory category
        /// </summary>
        public virtual Inventorycategories InventoryCategory { get; set; } 

    }
}
