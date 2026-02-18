using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Auth
{ 
   /// <summary>
   /// Login Authentication
   /// </summary>
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public string UserName { get; set; } = string.Empty;

        public int CompanyId { get; set; }
        public string? companyName { get; set; }
        public int CompanyClientId { get; set; }
        public string? companyClientName { get; set; }
        public int LocationId { get; set; }
        public string? LocationName { get; set; }
        public string? Enviromentname { get; set; }
        public string? RoleName { get; set; }
        public int companiesContractId { get; set; }
    }
}
