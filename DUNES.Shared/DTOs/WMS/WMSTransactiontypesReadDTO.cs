using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    public class WMSTransactiontypesReadDTO
    {


        /// <summary>
        /// internal database id
        /// </summary>
        /// 
        [Display(Name = "ID")]
        public int Id { get; set; }


        /// <summary>
        /// transaction name
        /// </summary>
        [MaxLength(200)]

        [Display(Name = "Name")]
        public string? Name { get; set; }

        /// <summary>
        /// company id
        /// </summary>
        /// 
        [Display(Name = "Company Id")]
        public int companyId { get; set; }

        /// <summary>
        /// company name
        /// </summary>
        /// 
        [Display(Name = "Company Name")]
        public string companyname { get; set; } = null!;


        /// <summary>
        /// this type is input
        /// </summary>
        /// 
        [Display(Name = "Is Output")]
        public bool Isinput { get; set; }

        /// <summary>
        /// this type is output
        /// </summary>
        /// 
        [Display(Name = "Is Output")]
        public bool Isoutput { get; set; }

        /// <summary>
        /// type is active
        /// </summary>
        /// 
        [Display(Name = "Is Active")]
        public bool Active { get; set; }

        /// <summary>
        /// code asociate for transaction type transfer
        /// </summary>
        [MaxLength(3)]
        [Display(Name ="Match")]
        public string? Match { get; set; }

      


    }
}
