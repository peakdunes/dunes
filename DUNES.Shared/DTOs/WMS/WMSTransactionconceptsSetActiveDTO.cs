using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Set-active DTO for transaction concepts master.
    /// </summary>
    public class WMSTransactionconceptsSetActiveDTO
    {
        /// <summary>
        /// Internal database id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// New active status.
        /// </summary>
        public bool Active { get; set; }
    }
}
