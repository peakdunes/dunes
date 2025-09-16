using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.WiewModels.Inventory
{

    /// <summary>
    /// USED for save bin distribution for a pick process
    /// </summary>
    public class BinPickWm
    {
        public int lineid { get; set; }  
        public int binidout { get; set; }
        public int qty { get; set; }
      
        /// <summary>
        /// new inventory after transaction
        /// </summary>
        public int typereserveid { get; set; }
        public int binidin { get; set; }
        public int statusid { get; set; }
        /// <summary>
        /// inventory original
        /// </summary>
        public int inventorytypeid { get; set; }
    }
}
