using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    public class WMSInventoryDetailByPartNumberDto
    {
        /// <summary>
        /// company id
        /// </summary>
        public int Idcompany { get; set; }

        /// <summary>
        /// company client
        /// </summary>
        [MaxLength(100)]
        public string companyclientid { get; set; } = string.Empty;

        /// <summary>
        /// location 
        /// </summary>
        public int locationid { get; set; }


        /// <summary>
        /// location description
        /// </summary>
        [MaxLength(100)]
        public string locationname { get; set; } = string.Empty;


        /// <summary>
        /// inventory qty
        /// </summary>
        public int qty { get; set; }


        /// <summary>
        /// Bins id
        /// </summary>
        public int binid { get; set; }

        /// <summary>
        /// Bin description
        /// </summary>
        [MaxLength(100)]
        public string binname { get; set; } = string.Empty;


        /// <summary>
        /// inventory type
        /// </summary>
        public int inventorytypeid { get; set; }

        /// <summary>
        /// inventory type description
        /// </summary>
        [MaxLength(100)]
        public string inventoryname { get; set; } = string.Empty;


        /// <summary>
        /// item status 
        /// </summary>
        public int statusid { get; set; }

        /// <summary>
        /// item status description
        /// </summary>
        [MaxLength(100)]
        public string statusname { get; set; } = string.Empty;

        /// <summary>
        /// Rack id
        /// </summary>
        public int rackid { get; set; }

        /// <summary>
        /// Rack Description
        /// </summary>
        [MaxLength(100)]
        public string rackname { get; set; } = string.Empty;

        /// <summary>
        /// Quantity reserved 
        /// </summary>
        public int qtyreserved { get; set; } = 0;
    }
}
