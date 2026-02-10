using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// DTO used to update an existing inventory category.
    /// </summary>
    public class WMSInventorycategoriesUpdateDTO
    {
        /// <summary>
        /// Identifier of the category to update.
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Updated name of the inventory category.
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Updated notes or observations.
        /// </summary>
        [MaxLength(1000)]
        public string? Observations { get; set; }

        /// <summary>
        /// Updated number of days between automatic cycle counts.
        /// </summary>
        [Range(0, 999)]
        public int CycleCountDays { get; set; }

        /// <summary>
        /// Updated error tolerance percentage.
        /// </summary>
        [Range(0, 100)]
        public decimal ErrorTolerance { get; set; }

        /// <summary>
        /// Indicates if the category should be marked as active.
        /// </summary>
        public bool Active { get; set; }
    }

}
