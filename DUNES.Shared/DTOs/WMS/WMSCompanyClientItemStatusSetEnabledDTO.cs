using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    public class WMSCompanyClientItemStatusSetEnabledDTO
    {
        public List<int> ItemStatusIds { get; set; } = new();
    }
}
