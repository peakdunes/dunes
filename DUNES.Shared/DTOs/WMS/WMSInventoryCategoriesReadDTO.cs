using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Read DTO for Inventory Category (includes navigation).
    /// </summary>
    public class WMSInventorycategoriesReadDTO
    {
        /// <summary>
        /// Internal identifier of the inventory category.
        /// </summary>
        /// 
        [Display(Name = "ID")]
        public int Id { get; set; }

        /// <summary>
        /// Name of the inventory category.
        /// </summary>
        /// 
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Company (tenant) identifier that owns the category.
        /// </summary>
        /// 
        [Display(Name = "Company ID")]
        public int CompanyId { get; set; }

        /// <summary>
        /// Optional notes or observations.
        /// </summary>
        /// 
        [Display(Name = "Observations")]
        public string? Observations { get; set; }

        /// <summary>
        /// Number of days between automatic cycle counts.
        /// </summary>
        /// 
        [Display(Name = "Cycle Count Days")]
        public int CycleCountDays { get; set; }

        /// <summary>
        /// Error tolerance percentage allowed during cycle count.
        /// </summary>
        /// 
        [Display(Name = "Error Tolerance")]
        public decimal ErrorTolerance { get; set; }

        /// <summary>
        /// Indicates if the category is currently active.
        /// </summary>
        /// 
        [Display(Name = "Active")]
        public bool Active { get; set; }

        /// <summary>
        /// Name of the company (navigation property).
        /// </summary>
        /// 
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; } = string.Empty;
    }

}
