using DUNES.API.ReadModels.Inventory;
using DUNES.Shared.TemporalModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Inventory
{
    public class PickProcessRequestDto
    {

        public required PickProcessHdr PickProcessHdr { get; set; }

        public required List<PickProcessItemDetail> ListItems { get; set; }

        public PickProcessCallsReadDto? CallsRead { get; set; }

        public WMSTransactionTm? ListTransactions { get; set; }

        public TorderRepairTm? OrderRepair { get; set; }

        public List<TzebB2bReplacementPartsInventoryLogDto>? ListInvMovZebra { get; set; }
        
    }

 
        
}
