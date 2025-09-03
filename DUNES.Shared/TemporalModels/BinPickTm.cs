using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.TemporalModels
{
    public class BinPickTm
    {
        public int lineid { get; set; }  // opcional si ya lo pasas arriba
        public int binid { get; set; }
        public int qty { get; set; }
    }
}
