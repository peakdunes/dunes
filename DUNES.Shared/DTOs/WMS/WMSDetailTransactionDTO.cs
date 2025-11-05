using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// DTO for InventorytransactionDetail
    /// </summary>
    public class WMSDetailTransactionDTO
    {

        [Display(Name ="Transaction Detail Id")]
        public int Id { get; set; }

        [Display(Name = "Transaction Type Id")]
        public int Idtypetransaction { get; set; }
        [Display(Name = "Transaction Type Description")]
        [MaxLength(200)]
        public string typetransactionName { get; set; } = string.Empty;

        [Display(Name = "Location Id")]
        public int Idlocation { get; set; }

        [Display(Name = "Location Name")]
        [MaxLength(200)]
        public string locationName { get; set; } = string.Empty;

        [Display(Name = "Inventory Type Id")]
        public int Idtype { get; set; }

        [Display(Name = "Inventory Type Name")]
        [MaxLength(200)]
        public string typeName { get; set; } = string.Empty;

        [Display(Name = "Rack Id")]
        public int Idrack { get; set; }

        [Display(Name = "Rack Name")]
        [MaxLength(200)]
        public string rackName { get; set; } = string.Empty;

        [Display(Name = "Level")]
        public int Level { get; set; }

        [Display(Name = "Part Number")]
        public string? Iditem { get; set; }

        [Display(Name = "Quantity")]
        public int TotalQty { get; set; }

        [Display(Name = "Bin Id")]
        public int Idbin { get; set; }

        [Display(Name = "Bin Name")]
        [MaxLength(200)]
        public string binName { get; set; } = string.Empty;

        [Display(Name = "Item Status Id")]
        public int Idstatus { get; set; }
        [Display(Name = "Item Status Description")]
        [MaxLength(200)]
        public string statusName { get; set; } = string.Empty;

        [Display(Name = "Serial Number")]
        public string? Serialid { get; set; }

        [Display(Name = "Company Id")]
        public int Idcompany { get; set; }
        [MaxLength(200)]
        [Display(Name = "Company Name")]
        public string companyName { get; set; } = string.Empty;
        [Display(Name = "Company Client")]
        public string? Idcompanyclient { get; set; }
        [Display(Name = "Company Client Division")]
        public string? Iddivision { get; set; }
        [Display(Name = "Header Transaction Id")]
        public int Idenctransaction { get; set; }
    }
}
