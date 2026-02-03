using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    public class WMSRacksCreateDTO
    {
      
        /// <summary>
        /// rack Name
        /// </summary>
        [Required]
        [Display(Name = "Rack Name")]
        public required string Name { get; set; }
             
       


        /// <summary>
        /// this rack is active
        /// </summary>
        [Display(Name = "Is Active")]
        public bool Active { get; set; }

    

    }
}
