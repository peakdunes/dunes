using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    public class WMSCreateHeaderTransactionDTO
    {
       
        public int Idcompany { get; set; }

        public int Idtransactionconcept { get; set; }

        public string? IdUser { get; set; }

        public string? IdUserprocess { get; set; }

        public int Idcompanyclient { get; set; }

        public string? Codecompanyclient { get; set; }

        [MaxLength(200)]
        public string? Documentreference { get; set; }

        [MaxLength(500)]
        public string? Observations { get; set; }

        public string? Iddivision { get; set; }
    }
}
