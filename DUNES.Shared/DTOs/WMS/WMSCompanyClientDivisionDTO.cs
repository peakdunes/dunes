using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{

    /// <summary>
    /// dto for CompanyClientDivision model
    /// </summary>
    public class WMSCompanyClientDivisionDTO
    {
        /// <summary>
        /// internal id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Division Name
        /// </summary>
        /// 
        [Required]
        [Display(Name ="Division Name")]
        public string? DivisionName { get; set; }

        /// <summary>
        /// Company client Id
        /// </summary>
        /// 
        [Required]
       [Display(Name ="Company Client")]
        public int Idcompanyclient { get; set; }


        /// <summary>
        /// Active
        /// </summary>
        /// 
        [Display(Name = "Active")]
        public bool IsActive { get; set; }
    }
}
