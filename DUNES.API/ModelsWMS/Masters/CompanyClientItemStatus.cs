using System.ComponentModel.DataAnnotations.Schema;

namespace DUNES.API.ModelsWMS.Masters
{
    /// <summary>
    /// Client configuration: enabled Item Status values for a given CompaniesContract (client profile).
    /// </summary>
    public class CompanyClientItemStatus
    {
        /// <summary>Identity.</summary>
        public int Id { get; set; }

        /// <summary>FK to CompaniesContract (client profile).</summary>
        public int CompaniesContractId { get; set; }

        /// <summary>FK to master Itemstatus.</summary>
        public int ItemStatusId { get; set; }

        /// <summary>
        /// Mapping-level active flag.
        /// Master also must be Active=true.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>Navigation: CompaniesContract.</summary>
        [ForeignKey(nameof(CompaniesContractId))]
        public virtual CompaniesContract CompaniesContractNavigation { get; set; } = null!;

        /// <summary>Navigation: Itemstatus master.</summary>
        [ForeignKey(nameof(ItemStatusId))]
        public virtual Itemstatus ItemStatusNavigation { get; set; } = null!;
    }
}
