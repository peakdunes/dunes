using DUNES.Shared.DTOs.WMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.TemporalModels
{
    public class WMSTransactionTm
    {

       public List<WMSHdrTransactionDTO> ListHdr { get; set; } = new List<WMSHdrTransactionDTO>();

       public List<WMSDetailTransactionDTO> ListDetail { get; set; } = new List<WMSDetailTransactionDTO>();

       public List<WMSInventoryMovementDTO> ListInventoryMovement { get; set; } = new List<WMSInventoryMovementDTO>();

    }
}
