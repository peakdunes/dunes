using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.WiewModels.Inventory
{
    //this class create the list information to create _TZEB_B2B_Replacement_Parts_Inventory_Log insert data
    //for a Pick Process 

    public class PickProcessWm
    {

        public int itemid { get; set; }
        public string? itemname { get; set; }
        public int InvSource { get; set; }  
        public string? sourcename { get; set;}
        public int InvDest { get; set; }
        public string? destname { get; set; }
        public int qty { get; set; }
        public int typetransactionid { get; set; }
        public string? match { get; set; }
        public string? serialnumber { get; set; }
        public string? notes { get; set; } = string.Empty;
    }
}
