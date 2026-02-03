using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    public class WMSRacksQueryDTO
    {

        /// <summary>
        /// internal DB ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// rack Name
        /// </summary>
        [Required]
        [Display(Name = "Rack Name")]
        public required string Name { get; set; }

        /// <summary>
        /// Company 
        /// </summary>
        [Required]
        [Display(Name = "Company")]
        public required int Idcompany { get; set; }
        public string CompanyName { get; set; } = null!;

        /// <summary>
        /// location when this rack is
        /// </summary>
        [Required]
        [Display(Name = "Location")]
        public int LocationsId { get; set; }
        public string LocationName { get; set; } = null!;

        /// <summary>
        /// this rack is active
        /// </summary>
        [Display(Name = "Is Active")]
        public bool Active { get; set; }

       
    }
}
