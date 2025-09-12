using DUNES.Shared.DTOs.WMS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.TemporalModels
{
    /// <summary>
    /// All information for create a inventory transaction
    /// </summary>

    public class NewInventoryTransactionTm
    {

        public required WMSCreateHeaderTransactionDTO hdr { get; set; }

        public required List<WMSCreateDetailTransactionDTO> Listdetails { get; set; }

       
    }
}
