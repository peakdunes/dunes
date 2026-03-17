using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        /// 
        [Display(Name ="Mapping ID")]
        public int Id { get; set; }

        /// <summary>
        /// Mapping company client identifier.
        /// </summary>
        public int CompanyClientId { get; set; }


        [Display(Name ="Transaction Concept ID")]
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
        [Display(Name = "Active")]
        public bool Active { get; set; }
    }
}
