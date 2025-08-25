using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Inventory
{
    public class WMSClientCompanies
    {
        public int Id { get; set; }           
        [MaxLength(200)]
        public string CompanyId { get; set; } = "";

    }
}
