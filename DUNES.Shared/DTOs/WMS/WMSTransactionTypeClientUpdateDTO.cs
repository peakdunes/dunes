using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// DTO used to update an existing Transaction Type
    /// to Company Client association.
    /// </summary>
    public class WMSTransactionTypeClientUpdateDTO
    {
        /// <summary>
        /// Mapping identifier.
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// master Id
        /// </summary>
        public int TransactionTypeId { get; set; }

        /// <summary>
        /// transaction type name
        /// </summary>
        public string TransactionTypeName { get; set; }

        /// <summary>
        /// Indicates whether this Transaction Type
        /// is active for the CompanyClient.
        /// </summary>
        public bool Active { get; set; }
    }
}
