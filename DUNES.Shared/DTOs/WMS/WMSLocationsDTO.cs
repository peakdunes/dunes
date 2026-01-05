using System;
using System.Collections.Generic;
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
        public int Id { get; set; }
        /// <summary>
        /// Location Description
        /// </summary>
        public string? Name { get; set; }
            
        /// <summary>
        /// Company ID
        /// </summary>
        public int Idcompany { get; set; }
        /// <summary>
        /// Country Id
        /// </summary>
        public int Idcountry { get; set; }
        /// <summary>
        /// State Id
        /// </summary>
        public int Idstate { get; set; }
        /// <summary>
        /// City Id
        /// </summary>
        public int Idcity { get; set; }
        /// <summary>
        /// Zip code
        /// </summary>
        public string? Zipcode { get; set; }
        /// <summary>
        /// Address
        /// </summary>
        public string? Address { get; set; }
        /// <summary>
        /// Phone
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// Is Active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// City name
        /// </summary>
        public string? cityname { get; set; }
        /// <summary>
        /// Company Name
        /// </summary>
        public string? companyname { get; set; }
        /// <summary>
        /// Country Name
        /// </summary>
        public string? countryname { get; set; }
        /// <summary>
        /// States Name
        /// </summary>
        public string? statename { get; set; }
      

    }
}
