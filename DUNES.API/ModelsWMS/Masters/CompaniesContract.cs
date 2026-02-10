using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DUNES.API.ModelsWMS.Masters
{

    /// <summary>
    /// company clients contract
    /// </summary>
    public class CompaniesContract
    {
        /// <summary>
        /// identity
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// company id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// company client Id
        /// </summary>
        public int CompanyClientId { get; set; }

        /// <summary>
        /// contract start date
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// contract end date
        /// </summary>
        public DateTime? EndDate { get; set; }
	
        /// <summary>
        /// contract is active
        /// </summary>
        public bool IsActive { get; set; }


        /// <summary>
        /// contract code
        /// </summary>
        [MaxLength(50)]
        public string? ContractCode { get; set; }

        /// <summary>
        /// User contract 
        /// </summary>
        [MaxLength(150)]
        public string? ContactName { get; set; }


        /// <summary>
        /// contract mail
        /// </summary>
        [MaxLength(150)]
        public string? ContactEmail { get; set; }

        /// <summary>
        /// Contract phone
        /// </summary>
        [MaxLength(150)]
        public string? ContactPhone { get; set; }

        /// <summary>
        /// Notes
        /// </summary>
        [MaxLength(50)]
        public string? Notes { get; set; }

        /// <summary>
        /// 0 = GenericOnly (nuestra tabla de items)
        /// 1 = ClientOnly (items del client)
        /// 2 = GenericPlusClient (items propios y del client)
        /// </summary>
        public int ItemCatalogMode { get; set; }


        /// <summary>
        /// Company Navegation
        /// </summary>
        [ForeignKey(nameof(CompanyId))]
        public virtual Company CompanyNavegation { get; set; } = null!;

        /// <summary>
        /// state navigation
        /// </summary>
        [ForeignKey(nameof(CompanyClientId))]
        public virtual CompanyClient CompanyClientNavegation { get; set; } = null!;

        /// <summary>
        /// client inventory type navegation
        /// </summary>
        public virtual ICollection<CompanyClientInventoryType> InventoryTypeMappings { get; set; } = new List<CompanyClientInventoryType>();
        /// <summary>
        /// client item status navegation
        /// </summary>
        public virtual ICollection<CompanyClientItemStatus> ItemStatusMappings { get; set; } = new List<CompanyClientItemStatus>();
       /// <summary>
       /// client categories navegation
       /// </summary>
        public virtual ICollection<CompanyClientInventoryCategory> InventoryCategoryMappings { get; set; } = new List<CompanyClientInventoryCategory>();


    }
}
