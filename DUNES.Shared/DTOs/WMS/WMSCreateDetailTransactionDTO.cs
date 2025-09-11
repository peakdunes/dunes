using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    public class WMSCreateDetailTransactionDTO
    {
      
        public int Idtypetransaction { get; set; }

        public int Idlocation { get; set; }

        public int Idtype { get; set; }

        public int Idrack { get; set; }

        public int Level { get; set; }

        public string? Iditem { get; set; }

        public int TotalQty { get; set; }

        public int Idbin { get; set; }

        public int Idstatus { get; set; }

        public string? Serialid { get; set; }

        public int Idcompany { get; set; }

        public string? Idcompanyclient { get; set; }

        public string? Iddivision { get; set; }

        public int Idenctransaction { get; set; }
    }
}
