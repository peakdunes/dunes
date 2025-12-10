using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    public class WMSCitiesReadDTO
    {
        /// <summary>
        /// internal id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// country id
        /// </summary>
        /// 
        [Required(ErrorMessage = "Country is required.")]
        [Display(Name = "Country Id")]
        public int Idcountry { get; set; }

        /// <summary>
        /// state id
        /// </summary>
        [Required(ErrorMessage = "State is required.")]
        [Display(Name = "State Id")]
        public int Idstate { get; set; }

        /// <summary>
        /// city name
        /// </summary>
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
        [Display(Name = "City Name")]
        public string? Name { get; set; }

        /// <summary>
        /// active
        /// </summary>
        public bool Active { get; set; }


        /// <summary>
        /// Navigation property for the related City entity.
        /// Allows access to full Bines details for this record.
        /// </summary>

        public virtual WMSCountriesDTO IdcountryNavigation { get; set; } = null!;


        /// <summary>
        /// Navigation property for the related City entity.
        /// Allows access to full Bines details for this record.
        /// </summary>

        public virtual WMSStatesCountriesDTO IdstateNavigation { get; set; } = null!;
    }
}
