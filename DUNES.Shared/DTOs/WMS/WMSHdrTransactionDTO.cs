using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{

    /// <summary>
    /// DTO for InventorytransactionHdr
    /// </summary>
    public class WMSHdrTransactionDTO
    {
        [Display(Name ="ID")]
        public int Id { get; set; }
        [Display(Name = "Company ID")]
        public int Idcompany { get; set; }
        [Display(Name = "Company Description")]
        [MaxLength(200)]
        public string CompanyName { get; set; } = string.Empty;
        [Display(Name = "Concept ID")]
        public int Idtransactionconcept { get; set; }
        [Display(Name = "Concept Transaction Description")]
        [MaxLength(200)]
        public string conceptName { get; set; } = string.Empty;
        [Display(Name = "User Created")]
        public string? IdUser { get; set; }
        [Display(Name = "Date Created")]
        public DateTime Datecreated { get; set; }
        [Display(Name = "Is Processed")]
        public bool Processed { get; set; }
        [Display(Name = "User Processed")]
        public string? IdUserprocess { get; set; }
        [Display(Name = "Company Client")]
        public string? Idcompanyclient { get; set; }
        [Display(Name = "Date Processed")]
        public DateTime Dateprocessed { get; set; }
        [Display(Name = "Document Reference")]
        public string? Documentreference { get; set; }

        public string? Observations { get; set; }

        public string? Iddivision { get; set; }
    }
}
