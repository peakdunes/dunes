using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Used to show all information included Country Information
    /// </summary>
    public class WMSStatesCountriesReadDTO
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
        [Display(Name = "Country ID")]
        public int Idcountry { get; set; }

        /// <summary>
        /// state name
        /// </summary>
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
        [Display(Name = "State Name")]
        public string? Name { get; set; }

        /// <summary>
        /// active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// ISO Country Code is required
        /// </summary>
        [Required(ErrorMessage = "ISO State Code is required.")]
        [MaxLength(5, ErrorMessage = "ISO State Code cannot exceed 5 characters.")]
        [Display(Name = "ISO State Code")]
        public string? Sigla { get; set; }



        /// <summary>
        /// Navigation property for the related City entity.
        /// Allows access to full Bines details for this record.
        /// </summary>

        public virtual WMSCountriesDTO IdcountryNavigation { get; set; } = null!;
    }
}
