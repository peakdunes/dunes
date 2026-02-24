using System.ComponentModel.DataAnnotations.Schema;

namespace DUNES.API.ModelsWMS.Masters
{
    /// <summary>
    /// Client configuration: enabled Item Status values for a given CompaniesContract (client profile).
    /// </summary>
    public class CompanyClientItemStatus
    {
        /// <summary>Identity (surrogate key).</summary>
        public int Id { get; set; }

        /// <summary>
        /// Company scope (multi-tenant).
        /// Value always comes from the token (never from body/query).
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Client scope within the company.
        /// Value always comes from the token (never from body/query).
        /// </summary>
        public int CompanyClientId { get; set; }

        /// <summary>
        /// FK to master Itemstatus catalog.
        /// Only master records with IsActive=true can be enabled for a client.
        /// </summary>
        public int ItemStatusId { get; set; }

        /// <summary>
        /// Mapping-level active flag (client enable/disable).
        /// Note: master catalog must also be IsActive=true to be considered enabled.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Navigation: Client entity (optional but useful for FK integrity and joins).
        /// </summary>
      
        public virtual CompanyClient CompanyClientNavigation { get; set; } = null!;

        /// <summary>Navigation: Itemstatus master catalog.</summary>
      
        public virtual Itemstatus ItemStatusNavigation { get; set; } = null!;
    }
}
