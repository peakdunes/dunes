using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        /// <summary>Mapping ID.</summary>
        public int Id { get; set; }

        /// <summary>Master InventoryCategory ID.</summary>
        /// 
        [Display(Name ="Category ID")]
        public int InventoryCategoryId { get; set; }

        /// <summary>Category name (from master catalog).</summary>
        /// 
        [Display(Name = "Category Name")]
        public string InventoryCategoryName { get; set; } = string.Empty;

        /// <summary>Mapping-level enable flag.</summary>
        /// 
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
}
