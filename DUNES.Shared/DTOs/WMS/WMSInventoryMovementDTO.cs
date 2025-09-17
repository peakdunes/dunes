using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{

    /// <summary>
    /// DTO for Inventorymovement
    /// </summary>
    public class WMSInventoryMovementDTO
    {

        public int Id { get; set; }

        public int Idtransactiontype { get; set; }
        
        [MaxLength(200)]
        public string transactionTypeName { get; set; } = string.Empty;

        public int Idlocation { get; set; }
        [MaxLength(200)]
        public string locationName { get; set; } = string.Empty;

        public int Idtype { get; set; }

        [MaxLength(200)]
        public string inventoryTypeName { get; set; } = string.Empty;

        public int Idrack { get; set; }

        public int Level { get; set; }

        public int Idbin { get; set; }

        [MaxLength(200)]
        public string binName { get; set; } = string.Empty;

        public string? Iditem { get; set; }

        public int Idstatus { get; set; }

        [MaxLength(200)]
        public string statusName { get; set; } = string.Empty;

        public string? Serialid { get; set; }

        public DateTime Datecreated { get; set; }

        public int Qtyinput { get; set; }

        public int Qtyoutput { get; set; }

        public int Qtybalance { get; set; }

        public int Idcompany { get; set; }

        [MaxLength(200)]
        public string companyName { get; set; } = string.Empty;

        public string? Idcompanyclient { get; set; }

        public int IdtransactionHead { get; set; }

        public int IdtransactionDetail { get; set; }

        public string? Iddivision { get; set; }

        public string? Createdby { get; set; }

        public int Idtransactionconcept { get; set; }

        [MaxLength(200)]
        public string conceptName { get; set; } = string.Empty;
    }
}
