using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Read DTO for ItemStatus mappings assigned to a client.
    /// </summary>
    public class WMSCompanyClientItemStatusReadDTO
    {
        /// <summary>
        /// Mapping identifier (surrogate key).
        /// </summary>
        /// 
        [Display(Name ="Mapped ID")]
        public int Id { get; set; }

        /// <summary>
        /// FK to master Itemstatus catalog.
        /// </summary>
        /// 
        [Display(Name = "Item Status")]
        public int ItemStatusId { get; set; }

        /// <summary>
        /// Display name from master Itemstatus catalog.
        /// </summary>
        /// 
        [Display(Name = "Item Status Name")]
        public string ItemStatusName { get; set; } = string.Empty;

        /// <summary>
        /// Mapping-level active flag.
        /// </summary>
        /// 
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
}
