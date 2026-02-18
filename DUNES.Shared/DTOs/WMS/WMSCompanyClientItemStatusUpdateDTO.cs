using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// DTO used to update the active status of a client-item status mapping.
    /// </summary>
    public class WMSCompanyClientItemStatusUpdateDTO
    {
        /// <summary>Mapping ID.</summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        /// <summary>Mapping-level enable flag.</summary>
        public bool IsActive { get; set; } = true;
    }
}
