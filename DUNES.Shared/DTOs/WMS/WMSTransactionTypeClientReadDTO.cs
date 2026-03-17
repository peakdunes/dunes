using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Represents the association between a Transaction Type
    /// and a Company Client, including descriptive names
    /// for UI and administrative purposes.
    /// </summary>
    public class WMSTransactionTypeClientReadDTO
    {
        /// <summary>
        /// Mapping identifier.
        /// </summary>
        /// 

        [Display(Name ="ID")]
        public int Id { get; set; }

        /// <summary>
        /// Company identifier (tenant).
        /// </summary>
        public int CompanyId { get; set; }


        /// <summary>
        /// Transaction Type identifier.
        /// </summary>
        /// 
        [Display(Name = "Transaction Type ID")]
        public int TransactionTypeId { get; set; }

        /// <summary>
        /// Transaction Type name.
        /// </summary>
        /// 
        [Display(Name = "Transaction Type Name")]
        public string TransactionTypeName { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether this Transaction Type
        /// is active for the CompanyClient.
        /// </summary>
        /// 
        [Display(Name = "Is Active")]
        public bool Active { get; set; }
    }
}
