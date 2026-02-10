using System;
using System.Collections.Generic;
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
        public int Id { get; set; }

        /// <summary>
        /// Name of the inventory category.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Company (tenant) identifier that owns the category.
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Optional notes or observations.
        /// </summary>
        public string? Observations { get; set; }

        /// <summary>
        /// Number of days between automatic cycle counts.
        /// </summary>
        public int CycleCountDays { get; set; }

        /// <summary>
        /// Error tolerance percentage allowed during cycle count.
        /// </summary>
        public decimal ErrorTolerance { get; set; }

        /// <summary>
        /// Indicates if the category is currently active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Name of the company (navigation property).
        /// </summary>
        public string CompanyName { get; set; } = string.Empty;
    }

}
