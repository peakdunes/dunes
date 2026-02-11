using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Update DTO for client-inventory type mapping.
    /// </summary>
    public class WMSCompanyClientInventoryTypeUpdateDTO
    {
        /// <summary>
        /// Mapping identity (required).
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Inventory type ID (must stay the same).
        /// </summary>
        [Required]
        public int InventoryTypeId { get; set; }

        /// <summary>
        /// Flag to activate or deactivate the mapping.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
