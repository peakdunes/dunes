using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{

    /// <summary>
    /// DTO for Inventorymovement
    /// </summary>
    public class WMSInventoryMovementDTO
    {
        [Display(Name ="Inventory Transaction Id")]
        public int Id { get; set; }

        [Display(Name = "Inventory Transaction Type")]
        public int Idtransactiontype { get; set; }

        [Display(Name = "Inventory Transaction Name")]
        [MaxLength(200)]
        public string transactionTypeName { get; set; } = string.Empty;

        [Display(Name = "Location Id")]
        public int Idlocation { get; set; }
        [MaxLength(200)]
        [Display(Name = "Location Name")]
        public string locationName { get; set; } = string.Empty;

        [Display(Name = "Inventory Type Id")]
        public int Idtype { get; set; }

        [Display(Name = "Inventory Type Name")]
        [MaxLength(200)]
        public string inventoryTypeName { get; set; } = string.Empty;

        [Display(Name = "Rack Id")]
        public int Idrack { get; set; }

        [Display(Name = "Level")]
        public int Level { get; set; }

        [Display(Name = "Bin Id")]
        public int Idbin { get; set; }

        [Display(Name = "Bin Name")]
        [MaxLength(200)]
        public string binName { get; set; } = string.Empty;

        [Display(Name = "Part Number")]
        public string? Iditem { get; set; }

        [Display(Name = "Item Status Id")]
        public int Idstatus { get; set; }

        [Display(Name = "Item Status Name")]
        [MaxLength(200)]
        public string statusName { get; set; } = string.Empty;

        [Display(Name = "Serial Number")]
        public string? Serialid { get; set; }

        [Display(Name = "Date Created")]
        public DateTime Datecreated { get; set; }

        [Display(Name = "Quantity Input")]
        public int Qtyinput { get; set; }

        [Display(Name = "Quantity Output")]
        public int Qtyoutput { get; set; }

        [Display(Name = "Quantity Balance")]
        public int Qtybalance { get; set; }

        [Display(Name = "Company Id")]
        public int Idcompany { get; set; }

        [Display(Name = "Company Name")]
        [MaxLength(200)]
        public string companyName { get; set; } = string.Empty;

        [Display(Name = "Company Client")]
        public string? Idcompanyclient { get; set; }

        [Display(Name = "Header Transaction Id")]
        public int IdtransactionHead { get; set; }

        [Display(Name = "Detail Transaction Id")]
        public int IdtransactionDetail { get; set; }

        [Display(Name = "Company Client Division")]
        public string? Iddivision { get; set; }

        [Display(Name = "Created by")]
        public string? Createdby { get; set; }

        [Display(Name = "Transaction Concept Id")]
        public int Idtransactionconcept { get; set; }

        [Display(Name = "Transaction Concept Name")]
        [MaxLength(200)]
        public string conceptName { get; set; } = string.Empty;
    }
}
