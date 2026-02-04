using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DUNES.API.ModelsWMS.Masters
{
    /// <summary>
    /// Defines the relationship between a Transaction Type and a Company Client.
    ///
    /// This table determines which Transaction Types are enabled
    /// for a specific CompanyClient within a Company (tenant).
    ///
    /// IMPORTANT:
    /// - Configuration table only (no business logic).
    /// - Uses STANDARD COMPANYID.
    /// - Contains only identifiers and activation state.
    /// </summary>
    [Table("TransactionTypeClients")]
    public class TransactionTypeClient
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
        /// Transaction Type identifier (master table).
        /// </summary>
        [Required]
        public int TransactionTypeId { get; set; }

        /// <summary>
        /// Indicates whether the Transaction Type
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
        /// Navigation to Transaction Type master.
        /// Passive reference only.
        /// </summary>
        public virtual Transactiontypes? TransactionType { get; set; }

        #endregion
    }
}
