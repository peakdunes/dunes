using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    public class WMSCountriesDTO
    {
        /// <summary>
        /// internal id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ISO Country Code
        /// </summary>
        [Required(ErrorMessage = "ISO Country Code is required.")]
        [MaxLength(5, ErrorMessage = "ISO Country Code cannot exceed 5 characters.")]
        [Display(Name = "ISO Country Code")]
        public string? Sigla { get; set; }

        /// <summary>
        /// country name
        /// </summary>
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
        [Display(Name = "Country Name")]
        public string? Name { get; set; }

        /// <summary>
        /// Active
        /// </summary>
        public bool Active { get; set; }
    }
}
