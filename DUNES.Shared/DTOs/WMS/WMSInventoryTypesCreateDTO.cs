using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Create DTO for Inventory Types (WMS).
    /// STANDARD COMPANYID: CompanyId is NOT accepted from client.
    /// </summary>
    public class WMSInventoryTypesCreateDTO
    {
        /// <summary>
        /// Type name.
        /// </summary>
        [Required]
        [MaxLength(255)]

        [Display(Name = "Name")]
        public string? Name { get; set; }

        /// <summary>
        /// Observations (max 1000 chars).
        /// </summary>
        [MaxLength(1000)]

        [Display(Name = "Observations")]
        public string? Observations { get; set; }

        /// <summary>
        /// Indicates if this inventory type represents On-Hand inventory.
        /// </summary>
        /// 
        [Display(Name = "Is onHand")]
        public bool IsOnHand { get; set; }

        /// <summary>
        /// Zebra integration association flag/value.
        /// </summary>
        /// 
        [Display(Name = "ZEBRA Inv Associated")]
        public int Zebrainvassociated { get; set; }

        [Display(Name ="Active")]
        public bool Active { get; set; }
    }
}
