using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        /// 
        [Display(Name="Concept Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Optional observations or notes.
        /// </summary>
        /// 
        [Display(Name = "Observations (1000)")]
        public string? Observations { get; set; }

        /// <summary>
        /// Indicates whether the concept is active.
        /// Default value is true.
        /// </summary>
        /// 

        [Display(Name = "Is Active")]
        public bool Active { get; set; } = true;
    }
}
