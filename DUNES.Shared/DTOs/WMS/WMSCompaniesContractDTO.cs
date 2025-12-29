using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    public class WMSCompaniesContractDTO
    {
        /// <summary>
        /// identity
        /// </summary>
        /// 
        [Display(Name ="ID")]
        public int Id { get; set; }

        /// <summary>
        /// company id
        /// </summary>
        /// 
        [Display(Name = "Company")]
        [Required]
        public int CompanyId { get; set; }

        /// <summary>
        /// company client Id
        /// </summary>
        /// 
        [Display(Name = "Company Client")]
        [Required]
        public int CompanyClientId { get; set; }

        /// <summary>
        /// contract start date
        /// </summary>
        /// 
        [Display(Name = "Start Date")]
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// contract end date
        /// </summary>
        /// 
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; } 

        /// <summary>
        /// contract is active
        /// </summary>
        /// 
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }


        /// <summary>
        /// contract code
        /// </summary>
        [MaxLength(50)]

        [Display(Name = "Contract Number")]
        [Required]
        public string? ContractCode { get; set; }

        /// <summary>
        /// User contract 
        /// </summary>
        [MaxLength(150)]
        [Display(Name = "Contact Name")]
        [Required]
        public string? ContactName { get; set; }


        /// <summary>
        /// contract mail
        /// </summary>
        [MaxLength(150)]

        [Display(Name = "Contact Mail")]
        [Required]
        public string? ContactEmail { get; set; }

        /// <summary>
        /// Contract phone
        /// </summary>
        [MaxLength(150)]

        [Display(Name = "Contact Phone")]
        [Required]
        public string? ContactPhone { get; set; }

        /// <summary>
        /// Notes
        /// </summary>
        [MaxLength(500)]

        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        [Display(Name = "Item Catalog Mode")]
        public int ItemCatalogMode { get; set; }
    }
}
