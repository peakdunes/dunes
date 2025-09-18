using DUNES.Shared.DTOs.B2B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.TemporalModels
{
    public class TorderRepairTm
    {
        public TorderRepairHdrDto? repairHdr {  get; set; }

        public List<TorderRepairItemsDto> ListItems { get; set; } = new();
        
        public List<TorderRepairItemsSerialsReceivingDto> ListItemsSerialsReceiving { get; set; } = new();

        public List<TorderRepairItemsSerialsShippingDto> ListItemsSerialsShipping { get; set; } = new();

    }
}
