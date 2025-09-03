using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.TemporalModels
{
    public class UpdateQuantityPickProcessRequestTm
    {

        public string companyclient { get; set; }
        public int lineid { get; set; }
        public List<BinPickTm> itemslist { get; set; } = new();
    }
}
