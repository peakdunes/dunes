using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    public class WMSCompanyClientDivisionReadDTO
    {
        /// <summary>
        /// internal id
        /// </summary>
        /// 
        [Display(Name ="ID")]
        public int Id { get; set; }

        /// <summary>
        /// Division Name
        /// </summary>
        /// 

       
        [Display(Name = "Division Name")]
        public string? DivisionName { get; set; }

        /// <summary>
        /// Company client Id
        /// </summary>
        /// 
        
        [Display(Name = "Company Client Id")]
        public int Idcompanyclient { get; set; }


        /// <summary>
        /// Active
        /// </summary>
        /// 
        [Display(Name = "Active")]
        public bool IsActive { get; set; }


        /// <summary>
        /// Company client name (from CompanyClient table)
        /// </summary>
        /// 
        [Display(Name = "Company Client Name")]
        public string? CompanyClientName { get; set; }
    }
}
