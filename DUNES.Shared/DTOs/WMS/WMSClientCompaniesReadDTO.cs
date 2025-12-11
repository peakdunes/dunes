using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{

    /// <summary>
    /// add navegation properties to show in List(s) view
    /// </summary>
    public class WMSClientCompaniesReadDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Company Legal Identification")]
        [Display(Name = "Identification ID")]
        public string? CompanyId { get; set; }

        [Required(ErrorMessage = "Company Name is required")]
        [Display(Name = "Company Name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Country company is required")]
        [Display(Name = "Country")]
        public int Idcountry { get; set; }

        [Required(ErrorMessage = "State company is required")]
        [Display(Name = "State")]
        public int Idstate { get; set; }

        [Required(ErrorMessage = "City company is required")]
        [Display(Name = "City")]
        public int Idcity { get; set; }

        [Required(ErrorMessage = "Zipcode company is required")]
        [Display(Name = "Zip Code")]
        public string? Zipcode { get; set; }

        [Required(ErrorMessage = "Address company is required")]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Phone company is required")]
        [Display(Name = "Phone")]
        public string? Phone { get; set; }

        [Display(Name = "Web Site")]
        public string? Website { get; set; }

        [Display(Name = "Active")]
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


        /// <summary>
        /// Navigation property for the related City entity.
        /// Allows access to full Bines details for this record.
        /// </summary>

        public virtual WMSCitiesDTO IdcityNavigation { get; set; } = null!;
    }
}
