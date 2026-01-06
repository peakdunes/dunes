using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// DTO table Locations
    /// </summary>
    public class WMSLocationsDTO
    {

        /// <summary>
        /// Location Id
        /// </summary>
        /// 
        [Display(Name ="ID")]
        public int Id { get; set; }
        /// <summary>
        /// Location Description
        /// </summary>
        ///    
        [Display(Name = "Location Name")]
        public string? Name { get; set; }

        /// <summary>
        /// Company ID
        /// </summary>
        /// 
        [Display(Name = "Company ID")]
        public int Idcompany { get; set; }
        /// <summary>
        /// Country Id
        /// </summary>
        /// 
        [Display(Name = "Country ID")]
        public int Idcountry { get; set; }
        /// <summary>
        /// State Id
        /// </summary>
        /// 
        [Display(Name = "State ID")]
        public int Idstate { get; set; }
        /// <summary>
        /// City Id
        /// </summary>
        /// 
        [Display(Name = "City ID")]
        public int Idcity { get; set; }
        /// <summary>
        /// Zip code
        /// </summary>
        /// 
        [Display(Name = "Zip Code")]
        public string? Zipcode { get; set; }
        /// <summary>
        /// Address
        /// </summary>
        /// 
        [Display(Name = "Location Address")]
        public string? Address { get; set; }
        /// <summary>
        /// Phone
        /// </summary>
        /// 
        [Display(Name = "Location Phone")]
        public string? Phone { get; set; }
        /// <summary>
        /// Is Active
        /// </summary>
        /// 
        [Display(Name = "Is Active")]
        public bool Active { get; set; }

        /// <summary>
        /// City name
        /// </summary>
        /// 
        [Display(Name = "City Name")]
        public string? cityname { get; set; }
        /// <summary>
        /// Company Name
        /// </summary>
        /// 
        [Display(Name = "Company Name")]
        public string? companyname { get; set; }
        /// <summary>
        /// Country Name
        /// </summary>
        /// 
        [Display(Name = "Country Name")]
        public string? countryname { get; set; }
        /// <summary>
        /// States Name
        /// </summary>
        /// 
        [Display(Name = "State Name")]
        public string? statename { get; set; }
      

    }
}
