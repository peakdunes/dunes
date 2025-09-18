using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Inventory
{

    /// <summary>
    ///  Show Id number of the ServTrack tables (_torderrepair...), calls tables y WMS transaction tables,
    ///  exist a method than create servtrack orders, calls and WMS transactions and it result are this numbers
    /// </summary>
    public class PickProcessResponseDto
    {

        public int ServTrackOrder { get; set; } = 0;

        public int WMSTransactionNumber { get; set; } = 0;

        public int Call13Number { get; set; } = 0;
    }
}
