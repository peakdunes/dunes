using DUNES.API.Data;
using DUNES.API.ReadModels.Inventory;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Repositories.Inventory.PickProcess.Queries
{
    /// <summary>
    /// All inventory pick process queries
    /// </summary>
    public class CommonQueryPickProcessINVRepository : ICommonQueryPickProcessINVRepository
    {

        private readonly AppDbContext _context;
        private readonly appWmsDbContext _wmscontext;

        /// <summary>
        /// initialize dbcontext
        /// </summary>
        /// <param name="context"></param>
        /// <param name="wmscontext"></param>
        public CommonQueryPickProcessINVRepository(AppDbContext context, appWmsDbContext wmscontext)
        {
            _context = context;
            _wmscontext = wmscontext;
        }
        /// <summary>
        /// Get ALl information about Pick Process
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PickProcessRead> GetPickProcessAllInfo(string DeliveryId)
        {
            var infoHdr = _context.TzebB2bPSoWoHdrTblItemInbConsReqsLog.FirstOrDefault(x => x.DeliveryId == DeliveryId);

            if (infoHdr == null)
            {
                return null;
            }

            var infodetail = await _context.TzebB2bPSoLineItemTblItemInbConsReqsLog.Where(x => x.PSoWoHdrTblItemId == infoHdr.Id).ToListAsync();

            if (infodetail.Count() <= 0)
            {
                return null;
            }

            PickProcessRead objdet = new PickProcessRead
            {
                pickHdr = infoHdr,
                pickdetails = infodetail

            };


            return objdet;
        }
    }
}
