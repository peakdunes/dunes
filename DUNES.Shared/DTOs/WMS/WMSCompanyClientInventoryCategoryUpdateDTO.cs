using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Update DTO for a client inventory category mapping.
    /// </summary>
    public class WMSCompanyClientInventoryCategoryUpdateDTO
    {
        /// <summary>
        /// Mapping identity (required for updates).
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Inventory category ID (must remain the same or validated on update).
        /// </summary>
        [Required]
        public int InventoryCategoryId { get; set; }

        /// <summary>
        /// Active flag to enable or disable this mapping.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
