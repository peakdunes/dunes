using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Inventory
{
    public class TzebB2bOutConsReqsInsertDto
    {
      
        public int TypeOfCallId { get; set; }
        
        public string? AdditionalInfo { get; set; }

        public string? TransactionCode { get; set; }
               
        public bool InProcess { get; set; }

        public string? Additional { get; set; }

    }
}
