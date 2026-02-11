using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Read DTO for client-level inventory category mapping.
    /// </summary>
    public class WMSCompanyClientInventoryCategoryReadDTO
    {
        /// <summary>
        /// Mapping identity.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Master inventory category ID.
        /// </summary>
        public int InventoryCategoryId { get; set; }

        /// <summary>
        /// Name of the inventory category.
        /// </summary>
        public string InventoryCategoryName { get; set; } = string.Empty;

        /// <summary>
        /// Active flag for this client mapping.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
