using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Inventory
{
    public class WMSItemByBinsDto
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public string Idcompanyclient { get; set; } = null!;

        public int BinesId { get; set; }

        public string Itemid { get; set; } = null!;
    }
}
