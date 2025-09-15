using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Inventory
{
    public class PickProcessResponseDto
    {

        public int ServTrackOrder { get; set; } = 0;

        public int WMSTransactionNumber { get; set; } = 0;

        public int Call13Number { get; set; } = 0;
    }
}
