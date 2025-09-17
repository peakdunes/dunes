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
        public int Id { get; set; }

        /// <summary>
        /// Part Number ID
        /// </summary>
        public int PartDefinitionId { get; set; }

        [MaxLength(200)]

        public string PartNumberName { get; set; } = string.Empty;
        /// <summary>
        /// Inventory Type ID Source
        /// </summary>
        public int InventoryTypeIdSource { get; set; }

        [MaxLength(200)]

        public string InvSourceName { get; set; } = string.Empty;
        /// <summary>
        /// Inventory Type ID Dest
        /// </summary>
        public int InventoryTypeIdDest { get; set; }


        [MaxLength(200)]

        public string InvDestName { get; set; } = string.Empty;
        /// <summary>
        /// Part Number serial id
        /// </summary>
        public string? SerialNo { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        public int Qty { get; set; }
        /// <summary>
        /// Notes
        /// </summary>
        public string? Notes { get; set; }
        /// <summary>
        /// Repair Number 
        /// </summary>
        public int? RepairNo { get; set; }
        /// <summary>
        /// Date Inserted
        /// </summary>
        public DateTime DateInserted { get; set; }
    }
}
