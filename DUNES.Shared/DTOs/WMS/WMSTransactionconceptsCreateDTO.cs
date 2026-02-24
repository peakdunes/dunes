using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Create DTO for transaction concepts master.
    /// </summary>
    public class WMSTransactionconceptsCreateDTO
    {
        /// <summary>
        /// Transaction concept name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Optional observations or notes.
        /// </summary>
        public string? Observations { get; set; }

        /// <summary>
        /// Indicates whether the concept is active.
        /// Default value is true.
        /// </summary>
        public bool Active { get; set; } = true;
    }
}
