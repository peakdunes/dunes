using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// DTO used to update an existing Transaction Concept
    /// to Company Client association.
    /// </summary>
    public class WMSTransactionConceptClientUpdateDTO
    {
        /// <summary>
        /// Mapping identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Mapping company client identifier.
        /// </summary>
        public int CompanyClientId { get; set; }

        /// <summary>
        /// Indicates whether this Transaction Concept
        /// is active for the CompanyClient.
        /// </summary>
        public bool Active { get; set; }
    }
}
