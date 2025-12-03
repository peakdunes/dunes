using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    public class WMSCompaniesDTO
    {

        /// <summary>
        /// primary key
        /// </summary>
        /// 
        [Display(Name="Id")]
        public int Id { get; set; }
        /// <summary>
        /// company id
        /// </summary>
        /// 
        [Display(Name = "Company Id")]
        public string? CompanyId { get; set; }
        /// <summary>
        /// company name
        /// </summary>
        /// 
        [Display(Name = "Company Name")]
        public string? Name { get; set; }
        /// <summary>
        /// country id
        /// </summary>
        /// 
        [Display(Name = "Country Id")]
        public int Idcountry { get; set; }
        /// <summary>
        /// state id
        /// </summary>
        /// 
        [Display(Name = "State Id")]
        public int Idstate { get; set; }
        /// <summary>
        /// city id
        /// </summary>
        /// 
        [Display(Name = "City Id")]
        public int Idcity { get; set; }
        /// <summary>
        /// zip code
        /// </summary>
        /// 
        [Display(Name = "Zip Code")]
        public string? Zipcode { get; set; }
        /// <summary>
        /// address
        /// </summary>
        /// 
        [Display(Name = "Address")]
        public string? Address { get; set; }
        /// <summary>
        /// phone
        /// </summary>
        /// 
        [Display(Name = "Phone Number")]
        public string? Phone { get; set; }
        /// <summary>
        /// web site
        /// </summary>
        /// 
        [Display(Name = "Web Site")]
        public string? Website { get; set; }

        /// <summary>
        /// is active
        /// </summary>
        /// 
        [Display(Name = "Is Active")]
        public bool Active { get; set; }

        [Display(Name = "Country Name")]
        public string? CountryName { get; set; }

        [Display(Name = "State Name")]
        public string? StateName { get; set; }

        [Display(Name = "City Name")]
        public string? CityName { get; set; }
    }
}
