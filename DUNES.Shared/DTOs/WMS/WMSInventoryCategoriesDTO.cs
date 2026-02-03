using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    public class WMSInventoryCategoriesDTO
    {

        ///// <summary>
        ///// internal database id
        ///// </summary>
        //public int Id { get; set; }

        /// <summary>
        /// category name
        /// </summary>
        public required string Name { get; set; }

        ///// <summary>
        ///// company id
        ///// </summary>
        //public int companyId { get; set; }

        /// <summary>
        /// boservations
        /// </summary>
        public string? Observations { get; set; }

        /// <summary>
        /// is active
        /// </summary>
        public bool Active { get; set; }


       

    }
}
