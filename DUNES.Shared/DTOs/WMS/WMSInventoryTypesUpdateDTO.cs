using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Update DTO for Inventory Types (WMS).
    /// STANDARD COMPANYID: CompanyId is NOT accepted from client.
    /// </summary>
    public class WMSInventoryTypesUpdateDTO
    {
        /// <summary>
        /// Database identifier.
        /// IMPORTANT: Controller forces this from route id (route is authoritative).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Type name.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string? Name { get; set; }

        /// <summary>
        /// Observations (max 1000 chars).
        /// </summary>
        [MaxLength(1000)]
        public string? Observations { get; set; }

        /// <summary>
        /// Indicates if this inventory type represents On-Hand inventory.
        /// </summary>
        public bool IsOnHand { get; set; }

        /// <summary>
        /// Zebra integration association flag/value.
        /// </summary>
        public int Zebrainvassociated { get; set; }
    }
}
