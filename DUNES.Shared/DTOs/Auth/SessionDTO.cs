using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Auth
{
    public class SessionDTO
    {
        public string Token { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;

        public int CompanyId { get; set; }
        public string? companyName { get; set; }
        public int CompanyClientId { get; set; }
        public string? companyClientName { get; set; }
        public int LocationId { get; set; }
        public string? LocationName { get; set; }
        
        public string? RoleName { get; set; }
                public DateTime Expiration { get; set; }

        // Ambiente
        public string Environment { get; set; } = "UAT"; // PROD | UAT | DEV
    }
}
