using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// DTO used to create a new inventory category.
    /// </summary>
    public class WMSInventorycategoriesCreateDTO
    {
        /// <summary>
        /// Name of the new inventory category.
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Optional notes or observations.
        /// </summary>
        [MaxLength(1000)]
        public string? Observations { get; set; }

        /// <summary>
        /// Number of days between automatic cycle counts.
        /// </summary>
        [Range(0, 999)]
        public int CycleCountDays { get; set; }

        /// <summary>
        /// Error tolerance percentage allowed during cycle count.
        /// </summary>
        [Range(0, 100)]
        public decimal ErrorTolerance { get; set; }
    }


}
