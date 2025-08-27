using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Inventory
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
        public string companyclientid { get; set; }

        /// <summary>
        /// location 
        /// </summary>
        public int locationid { get; set; }


        /// <summary>
        /// location description
        /// </summary>
        [MaxLength(100)]
        public string locationname { get; set; }


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
        public string binname { get; set; }


        /// <summary>
        /// inventory type
        /// </summary>
        public int inventorytypeid { get; set; }

        /// <summary>
        /// inventory type description
        /// </summary>
        [MaxLength(100)]
        public string inventoryname { get; set; }


        /// <summary>
        /// item status 
        /// </summary>
        public int statusid { get; set; }

        /// <summary>
        /// item status description
        /// </summary>
        [MaxLength(100)]
        public string statusname { get; set; }

        /// <summary>
        /// Rack id
        /// </summary>
        public int rackid { get; set; }

        /// <summary>
        /// Rack Description
        /// </summary>
        [MaxLength(100)]
        public string rackname { get; set; }
    }
}
