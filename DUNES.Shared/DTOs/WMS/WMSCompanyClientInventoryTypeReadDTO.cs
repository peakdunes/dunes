using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Read DTO for client-level inventory type mapping.
    /// </summary>
    public class WMSCompanyClientInventoryTypeReadDTO
    {
        /// Mapping record Id.
        /// </summary>
        /// 
        [Display(Name ="ID")]
        public int Id { get; set; }

        /// <summary>
        /// FK to master InventoryTypes catalog.
        /// </summary>
        /// 
        [Display(Name = "Inventory Type ID")]
        public int InventoryTypeId { get; set; }

        /// <summary>
        /// InventoryType display name from master catalog.
        /// </summary>
        /// 
        [Display(Name = "Inventory Name")]
        public string InventoryTypeName { get; set; } = string.Empty;

        /// <summary>
        /// Mapping-level active flag.
        /// </summary>
        /// 
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
}
