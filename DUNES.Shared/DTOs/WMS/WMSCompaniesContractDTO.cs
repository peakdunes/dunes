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
        public int Id { get; set; }

        /// <summary>
        /// company id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// company client Id
        /// </summary>
        public int CompanyClientId { get; set; }

        /// <summary>
        /// contract start date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// contract end date
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// contract is active
        /// </summary>
        public bool IsActive { get; set; }


        /// <summary>
        /// contract code
        /// </summary>
        [MaxLength(50)]
        public string? ContractCode { get; set; }

        /// <summary>
        /// User contract 
        /// </summary>
        [MaxLength(150)]
        public string? ContactName { get; set; }


        /// <summary>
        /// contract mail
        /// </summary>
        [MaxLength(150)]
        public string? ContactEmail { get; set; }

        /// <summary>
        /// Contract phone
        /// </summary>
        [MaxLength(150)]
        public string? ContactPhone { get; set; }

        /// <summary>
        /// Notes
        /// </summary>
        [MaxLength(50)]
        public string? Notes { get; set; }
    }
}
