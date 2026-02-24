using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Read DTO for transaction concepts master.
    /// </summary>
    public class WMSTransactionconceptsReadDTO
    {
        /// <summary>
        /// Internal database id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Transaction concept name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Company scope (comes from token in API logic).
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Optional observations or notes.
        /// </summary>
        public string? Observations { get; set; }

        /// <summary>
        /// Indicates whether the concept is active.
        /// </summary>
        public bool Active { get; set; }
    }
}
