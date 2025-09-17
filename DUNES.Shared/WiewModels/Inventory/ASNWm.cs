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
        public ASNHdr asnHdr { get; set; } = new();

        public List<ASNItemDetail> itemDetail { get; set; } = new();

    }


   
}
