using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    public class TransactiontypesReadDTO
    {


        /// <summary>
        /// internal database id
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// transaction name
        /// </summary>
        [MaxLength(200)]
        public string? Name { get; set; }

        /// <summary>
        /// company id
        /// </summary>
        public int companyId { get; set; }

        /// <summary>
        /// company name
        /// </summary>
        public string companyname { get; set; } = null!;


        /// <summary>
        /// this type is input
        /// </summary>
        public bool Isinput { get; set; }

        /// <summary>
        /// this type is output
        /// </summary>
        public bool Isoutput { get; set; }

        /// <summary>
        /// type is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// code asociate for transaction type transfer
        /// </summary>
        [MaxLength(3)]
        public string? Match { get; set; }

      


    }
}
