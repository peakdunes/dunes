using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Inventory
{

    /// <summary>
    /// show information for Pick Process and ASN Peack to ZEBRA calls
    /// </summary>
    public class TzebB2bOutConsReqsDto
    {
        public int Id { get; set; }

        public int TypeOfCallId { get; set; }

        public DateTime DateTimeInserted { get; set; }

        public bool? AckReceived { get; set; }

        public string? Result { get; set; }
               
        public DateTime? SentTimestamp { get; set; }

        public DateTime? DateTimeSent { get; set; }
        
        [MaxLength(200)]
        public string? callName { get; set; }
    }
}
