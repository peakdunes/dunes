using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    public class WMSTransactionConceptClientSetActiveDTO
    {
        public int Id { get; set; }

       
        public bool Active { get; set; } = true;
    }
}
