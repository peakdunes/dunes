using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// DTO for InventorytransactionDetail
    /// </summary>
    public class WMSDetailTransactionDTO
    {
        public int Id { get; set; }

        public int Idtypetransaction { get; set; }

        [MaxLength(200)]
        public string typetransactionName { get; set; } = string.Empty;

        public int Idlocation { get; set; }

        [MaxLength(200)]
        public string locationName { get; set; } = string.Empty;

        public int Idtype { get; set; }

        [MaxLength(200)]
        public string typeName { get; set; } = string.Empty;

        public int Idrack { get; set; }

        [MaxLength(200)]
        public string rackName { get; set; } = string.Empty;

        public int Level { get; set; }

        public string? Iditem { get; set; }

        public int TotalQty { get; set; }

        public int Idbin { get; set; }

        [MaxLength(200)]
        public string binName { get; set; } = string.Empty;

        public int Idstatus { get; set; }

        [MaxLength(200)]
        public string statusName { get; set; } = string.Empty;

        public string? Serialid { get; set; }


        public int Idcompany { get; set; }
        [MaxLength(200)]
        public string companyName { get; set; } = string.Empty;

        public string? Idcompanyclient { get; set; }

        public string? Iddivision { get; set; }

        public int Idenctransaction { get; set; }
    }
}
