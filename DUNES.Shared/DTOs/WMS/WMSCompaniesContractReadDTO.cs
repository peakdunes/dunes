using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    public class WMSCompaniesContractReadDTO
    {
        /// <summary>
        /// identity
        /// </summary>
        /// 
        [Display(Name = "ID")]
        public int Id { get; set; }

        /// <summary>
        /// company id
        /// </summary>
        /// 
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        /// <summary>
        /// company client Id
        /// </summary>
        /// 
        [Display(Name = "Company Client")]
        public int CompanyClientId { get; set; }

        /// <summary>
        /// contract start date
        /// </summary>
        /// 
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// contract end date
        /// </summary>
        /// 
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

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
        public string? ContractCode { get; set; }

        /// <summary>
        /// User contract 
        /// </summary>
        [MaxLength(150)]

        [Display(Name = "Contact Name")]
        public string? ContactName { get; set; }


        /// <summary>
        /// contract mail
        /// </summary>
        [MaxLength(150)]

        [Display(Name = "Contect Email")]
        public string? ContactEmail { get; set; }

        /// <summary>
        /// Contract phone
        /// </summary>
        [MaxLength(150)]

        [Display(Name = "Contact Phone")]
        public string? ContactPhone { get; set; }

        /// <summary>
        /// Notes
        /// </summary>
        [MaxLength(50)]

        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        /// <summary>
        /// 0 = GenericOnly (nuestra tabla de items)
        /// 1 = ClientOnly (items del client)
        /// 2 = GenericPlusClient (items propios y del client)
        /// </summary>
        /// 
        [Display(Name = "Item Catalog Mode")]
        public int ItemCatalogMode { get; set; }


        /// <summary>
        /// Company Navegation
        /// </summary>
        [ForeignKey(nameof(CompanyId))]
        public virtual WMSCompaniesDTO CompanyNavegation { get; set; } = null!;

        /// <summary>
        /// state navigation
        /// </summary>
        [ForeignKey(nameof(CompanyClientId))]
        public virtual WMSClientCompaniesDTO CompanyClientNavegation { get; set; } = null!;
    }
}
