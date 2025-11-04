using DUNES.Shared.DTOs.Inventory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.WiewModels.Inventory
{
    public class ASNWm
    {
        public ASNHdrDto asnHdr { get; set; } = new();

        public List<ASNItemDetailDto> itemDetail { get; set; } = new();

        public TzebB2bIrReceiptOutHdrDetItemInbConsReqsLogDto asnReceiptHdr { get; set; } = new();

        public List<TzebB2bIrReceiptLineItemTblItemInbConsReqsLogDto> asnReceiptList { get; set; } = new();

    }


   
}
