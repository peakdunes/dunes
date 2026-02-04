using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// DTO used to create a new association between
    /// a Transaction Concept and a Company Client.
    /// </summary>
    public class WMSTransactionConceptClientCreateDTO
    {
        /// <summary>
        /// Company client identifier.
        /// </summary>
        public int CompanyClientId { get; set; }

        /// <summary>
        /// Transaction concept identifier.
        /// </summary>
        public int TransactionConceptId { get; set; }

        /// <summary>
        /// Indicates whether the association
        /// should be created as active.
        /// </summary>
        public bool Active { get; set; } = true;
    }
}
