using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DUNES.API.ModelsWMS.Masters
{
    /// <summary>
    /// Defines the relationship between a Transaction Concept
    /// and a Company Client.
    ///
    /// This table determines which Transaction Concepts are enabled
    /// for a specific CompanyClient within a Company (tenant).
    ///
    /// IMPORTANT:
    /// - Configuration table only (no business logic).
    /// - Uses STANDARD COMPANYID.
    /// - Contains only identifiers and activation state.
    /// </summary>
    [Table("TransactionConceptClients")]
    public class TransactionConceptClient
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Company identifier (tenant).
        /// STANDARD COMPANYID.
        /// </summary>
        [Required]
        public int CompanyId { get; set; }

        /// <summary>
        /// Company client identifier.
        /// </summary>
        [Required]
        public int CompanyClientId { get; set; }

        /// <summary>
        /// Transaction Concept identifier (master table).
        /// </summary>
        [Required]
        public int TransactionConceptId { get; set; }

        /// <summary>
        /// Indicates whether the Transaction Concept
        /// is enabled for this CompanyClient.
        /// </summary>
        [Required]
        public bool Active { get; set; } = true;

        #region Navigation properties (passive)

        /// <summary>
        /// Navigation to Company (tenant).
        /// Passive reference only.
        /// </summary>
        public virtual Company? Company { get; set; }

        /// <summary>
        /// Navigation to CompanyClient.
        /// Passive reference only.
        /// </summary>
        public virtual CompanyClient? CompanyClient { get; set; }

        /// <summary>
        /// Navigation to Transaction Concept master.
        /// Passive reference only.
        /// </summary>
        public virtual Transactionconcepts? TransactionConcept { get; set; }

        #endregion
    }
}
