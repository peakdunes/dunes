using Microsoft.EntityFrameworkCore;

namespace APIZEBRA.Data
{
    /// <summary>
    /// Initializes a new instance of the <see cref="appWmsDbContext"/> class DBKWMS database using the specified options.
    /// </summary>
    public class appWmsDbContext : DbContext
    {
        /// <summary>
        /// Constructor that sets up the database context with the given options.
        /// </summary>
        /// <param name="options">Configuration settings for the DbContext.</param>
        public appWmsDbContext(DbContextOptions<appWmsDbContext> options) : base(options)
        {
        }
    }
}
