using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Inventory
{
    public class TzebB2bInbConsReqsDto
    {

        public int Id { get; set; }
      
        public string TransactionCode { get; set; } = null!;
        
        public DateTime SentTimestamp { get; set; }
        
        public DateTime DateTimeInserted { get; set; }

        public string? Error { get; set; }

        public bool Processed { get; set; }

    }
}
