using System.ComponentModel.DataAnnotations.Schema;

namespace DUNES.API.ModelsWMS.Masters
{
    /// <summary>
    /// Client configuration: enabled Inventory Categories for a given CompaniesContract (client profile).
    /// </summary>
    public class CompanyClientInventoryCategory
    {
        /// <summary>Identity.</summary>
        public int Id { get; set; }

        /// <summary>FK to CompaniesContract (client profile).</summary>
        public int CompaniesContractId { get; set; }

        /// <summary>FK to master Inventorycategories.</summary>
        public int InventoryCategoryId { get; set; }

        /// <summary>
        /// Mapping-level active flag.
        /// Master also must be Active=true.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>Navigation: CompaniesContract.</summary>
        [ForeignKey(nameof(CompaniesContractId))]
        public virtual CompaniesContract CompaniesContractNavigation { get; set; } = null!;

        /// <summary>Navigation: Inventorycategories master.</summary>
        [ForeignKey(nameof(InventoryCategoryId))]
        public virtual Inventorycategories InventoryCategoryNavigation { get; set; } = null!;
    }
}
