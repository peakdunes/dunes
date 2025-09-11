using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// If the order creation process was correct, return reference number, or
/// return cero and error message
/// </summary>
namespace DUNES.Shared.DTOs.Inventory
{
    [Keyless]
    public class ServTrackReferenceCreatedDto
    {
        /// <summary>
        /// Servtrack order number
        /// </summary>
        public int RefNum { get; set; }

        /// <summary>
        /// process error 
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}
