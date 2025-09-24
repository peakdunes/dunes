using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    public class WMSTransactionsDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string match { get; set; } = string.Empty;

        public bool isInput { get; set; }

        public bool isOutput { get; set; }
    }
}
