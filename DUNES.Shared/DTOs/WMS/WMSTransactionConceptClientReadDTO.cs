using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Represents the association between a Transaction Concept
    /// and a Company Client, including descriptive names
    /// for UI and administrative purposes.
    /// </summary>
    public class WMSTransactionConceptClientReadDTO
    {
        /// <summary>
        /// Mapping identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Company identifier (tenant).
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Company name.
        /// </summary>
        public string CompanyName { get; set; } = string.Empty;

        /// <summary>
        /// Company client identifier.
        /// </summary>
        public int CompanyClientId { get; set; }

        /// <summary>
        /// Company client name.
        /// </summary>
        public string CompanyClientName { get; set; } = string.Empty;

        /// <summary>
        /// Transaction concept identifier.
        /// </summary>
        public int TransactionConceptId { get; set; }

        /// <summary>
        /// Transaction concept name.
        /// </summary>
        public string TransactionConceptName { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether this Transaction Concept
        /// is active for the CompanyClient.
        /// </summary>
        public bool Active { get; set; }
    }
}
