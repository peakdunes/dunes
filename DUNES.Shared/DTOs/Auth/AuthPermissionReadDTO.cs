using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Auth
{
    public class AuthPermissionReadDTO
    {
        public int Id { get; set; }
        public string PermissionKey { get; set; } = string.Empty;
        [MaxLength(100)]
        public string GroupName { get; set; } = string.Empty;
        [MaxLength(100)]
        public string ModuleName { get; set; } = string.Empty;
        [MaxLength(100)]
        public string ActionName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedAt { get; set; }

     

       

        
      

     

      
       

      
     





    }
}
