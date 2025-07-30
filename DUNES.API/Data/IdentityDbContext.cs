using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Data
{
    public class IdentityDbContext: IdentityDbContext<IdentityUser>
    {

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options): base(options) 
        { 
        }
       
    }
}
