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
       
    }
}
