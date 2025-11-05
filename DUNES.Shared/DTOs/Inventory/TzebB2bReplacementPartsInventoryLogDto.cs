using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Inventory
{
    public class TzebB2bReplacementPartsInventoryLogDto
    {
        /// <summary>
        /// Record ID
        /// </summary>
        /// 
        [Display(Name ="Transaction Id")]
        public int Id { get; set; }

        /// <summary>
        /// Part Number ID
        /// </summary>
        /// 
        [Display(Name = "Item Id")]
        public int PartDefinitionId { get; set; }

        [MaxLength(200)]
        [Display(Name = "Part Number")]
        public string PartNumberName { get; set; } = string.Empty;
        /// <summary>
        /// Inventory Type ID Source
        /// </summary>
        /// 
        [Display(Name = "Inventory Source Id")]
        public int InventoryTypeIdSource { get; set; }

        [MaxLength(200)]
        [Display(Name = "Inventory Source Name")]
        public string InvSourceName { get; set; } = string.Empty;
        /// <summary>
        /// Inventory Type ID Dest
        /// </summary>
        /// 
        [Display(Name = "Inventory Dest Id")]
        public int InventoryTypeIdDest { get; set; }


        [MaxLength(200)]
        [Display(Name = "Inventory Source Name")]
        public string InvDestName { get; set; } = string.Empty;
        /// <summary>
        /// Part Number serial id
        /// </summary>
        /// 
        [Display(Name = "Serial Number")]
        public string? SerialNo { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        /// 
        [Display(Name = "Quantity")]
        public int Qty { get; set; }
        /// <summary>
        /// Notes
        /// </summary>
        /// 
        [Display(Name = "Notes")]
        public string? Notes { get; set; }
        /// <summary>
        /// Repair Number 
        /// </summary>
        /// 
        [Display(Name = "Repair Number")]
        public int? RepairNo { get; set; }
        /// <summary>
        /// Date Inserted
        /// </summary>
        /// 
        [Display(Name = "Date Inserted")]
        public DateTime DateInserted { get; set; }
    }
}
