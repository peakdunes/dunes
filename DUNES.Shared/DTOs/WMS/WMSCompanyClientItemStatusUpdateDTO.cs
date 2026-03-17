using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Request DTO to update an existing ItemStatus mapping for a client.
    /// </summary>
    public class WMSCompanyClientItemStatusUpdateDTO
    {
        /// <summary>
        /// Mapping identifier (surrogate key).
        /// </summary>
        /// 
        [Display(Name = "ID")]
        public int Id { get; set; }

        /// <summary>
        /// FK to master Itemstatus catalog.
        /// </summary>
        /// 
        [Display(Name = "Item Status ID")]
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
        public bool IsActive { get; set; }
    }
}
