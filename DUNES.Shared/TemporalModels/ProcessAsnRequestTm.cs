using DUNES.Shared.WiewModels.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.TemporalModels
{
    /// <summary>
    /// info class to create ASN receiving transaction
    /// </summary>
    public class ProcessAsnRequestTm
    {
        /// <summary>
        /// information to create WMS Transations (Header and detail
        /// </summary>
       public NewInventoryTransactionTm wmsInfo { get; set; }

        /// <summary>
        /// receiving bins distribution
        /// </summary>
       public  List<BinsToLoadWm> listdetail { get; set; }
    }
}
