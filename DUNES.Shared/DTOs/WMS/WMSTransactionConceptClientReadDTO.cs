using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        /// 
        [Display(Name ="Mapping ID")]
        public int Id { get; set; }
        

        /// <summary>
        /// Transaction concept identifier.
        /// </summary>
        /// 
        [Display(Name = "Transaction Concept ID")]
        public int TransactionConceptId { get; set; }

        /// <summary>
        /// Transaction concept name.
        /// </summary>
        /// 
        [Display(Name = "Transaction Concept Name")]
        public string TransactionConceptName { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether this Transaction Concept
        /// is active for the CompanyClient.
        /// </summary>
        /// 
        [Display(Name = "Is Active")]
        public bool Active { get; set; }
    }
}
