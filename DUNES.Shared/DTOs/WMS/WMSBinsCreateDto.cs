using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    public class WMSBinsCreateDto
    {

      

        /// <summary>
        /// rack Name
        /// </summary>
      
        [Display(Name = "Bin Name")]
        public required string Name { get; set; } = string.Empty;

      

        /// <summary>
        /// this rack is active
        /// </summary>
        [Display(Name = "Is Active")]
        public bool Active { get; set; }

     

    }
}
