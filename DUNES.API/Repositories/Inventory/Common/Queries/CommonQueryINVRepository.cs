using DUNES.API.Data;
using DUNES.API.ReadModels.Inventory;
using System.Runtime.InteropServices;

namespace DUNES.API.Repositories.Inventory.Common.Queries
{
    /// <summary>
    /// All common inventory queries
    /// </summary>
    public class CommonQueryINVRepository : ICommonQueryINVRepository
    {

        private readonly AppDbContext _context;

        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="context"></param>
        public CommonQueryINVRepository(AppDbContext context)
        {
            _context = context;
        }



        /// <summary>
        /// Get all inventory transactions for a Document Number
        /// </summary>
        /// <param name="DocumentNumber"></param>
        /// <returns></returns>
        public Task<ASNRead> GetAllInventoryTransactionsByDocument(string DocumentNumber)
        {
            var infotran = _context.TzebB2bReplacementPartsInventoryLog.Where(x => x.Notes.Contains(DocumentNumber)).ToList();
        }
    }
}
