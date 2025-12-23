using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Data
{
    /// <summary>
    /// identity db context
    /// </summary>
    public class IdentityDbContext: IdentityDbContext<IdentityUser>
    {


        /// <summary>
        /// DI
        /// </summary>
        /// <param name="options"></param>
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options): base(options) 
        { 
        }
       
    }
}
