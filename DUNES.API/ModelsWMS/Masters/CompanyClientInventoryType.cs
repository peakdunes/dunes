using System.ComponentModel.DataAnnotations.Schema;

namespace DUNES.API.ModelsWMS.Masters
{
    /// <summary>
    /// Client configuration: enabled Inventory Types for a given CompaniesContract (client profile).
    /// </summary>
    public class CompanyClientInventoryType
    {
        /// <summary>Identity.</summary>
        public int Id { get; set; }

        /// <summary>FK to CompaniesContract (client profile).</summary>
        public int CompaniesContractId { get; set; }

        /// <summary>FK to master InventoryTypes.</summary>
        public int InventoryTypeId { get; set; }

        /// <summary>
        /// Mapping-level active flag.
        /// Master also must be Active=true.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>Navigation: CompaniesContract.</summary>
        [ForeignKey(nameof(CompaniesContractId))]
        public virtual CompaniesContract CompaniesContractNavigation { get; set; } = null!;

        /// <summary>Navigation: InventoryTypes master.</summary>
        [ForeignKey(nameof(InventoryTypeId))]
        public virtual InventoryTypes InventoryTypeNavigation { get; set; } = null!;
    }
}
