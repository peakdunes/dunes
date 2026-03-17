using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        /// Transaction concept identifier.
        /// </summary>
        /// 
        [Display(Name ="Transaction Concept Name")]
        public int TransactionConceptId { get; set; }

        /// <summary>
        /// Indicates whether the association
        /// should be created as active.
        /// </summary>
        /// 
        [Display(Name = "Active")]
        public bool Active { get; set; } = true;
    }
}
