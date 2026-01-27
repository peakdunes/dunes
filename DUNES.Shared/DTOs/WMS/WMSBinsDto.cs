using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    public class WMSBinsDto
    {

        /// <summary>
        /// internal DB ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// rack Name
        /// </summary>
      
        [Display(Name = "Rack Name")]
        public required string Name { get; set; } = string.Empty;

        /// <summary>
        /// Company 
        /// </summary>
       
        [Display(Name = "Company")]
        public required int Idcompany { get; set; }

        /// <summary>
        /// location when this rack is
        /// </summary>
      
        [Display(Name = "Location")]
        public int LocationsId { get; set; }

        /// <summary>
        /// rack
        /// </summary>

        [Required]
        [Display(Name = "Racks")]
        public int RacksId { get; set; }

        /// <summary>
        /// this rack is active
        /// </summary>
        [Display(Name = "Is Active")]
        public bool Active { get; set; }

        /// <summary>
        /// company navegation property
        /// </summary>
        public virtual WMSCompaniesDTO IdcompanyNavigation { get; set; } = null!;

        /// <summary>
        /// location navegation property
        /// </summary>
        public virtual WMSLocationsDTO Locations { get; set; } = null!;


    }
}
