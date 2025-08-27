using DUNES.API.Data;
using DUNES.API.ReadModels.Inventory;
using DUNES.Shared.DTOs.Inventory;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Repositories.Inventory.Common.Queries
{
    /// <summary>
    /// Common Inventory Queries
    /// </summary>
    public class CommonQueryINVRepository : ICommonQueryINVRepository
    {

        private readonly AppDbContext _context;
        private readonly appWmsDbContext _wmscontext;

        /// <summary>
        /// initialize dbcontext
        /// </summary>
        /// <param name="context"></param>
        /// <param name="wmscontext"></param>
        public CommonQueryINVRepository(AppDbContext context, appWmsDbContext wmscontext)
        {
            _context = context;
            _wmscontext = wmscontext;
        }

        /// <summary>
        /// Get All information about ASN Process
        /// </summary>
        /// <param name="ShipmentNum"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ASNRead> GetASNAllInfo(string ShipmentNum)
        {

            var infoHdr = _context.TzebB2bAsnOutHdrDetItemInbConsReqs.FirstOrDefault(x => x.ShipmentNum == ShipmentNum);

            if (infoHdr == null)
            {
                return null;
            }

            var infodetail = await _context.TzebB2bAsnLineItemTblItemInbConsReqs.Where(x => x.AsnOutHdrDetItemId == infoHdr.Id).ToListAsync();

            if (infodetail.Count() <= 0)
            {
                return null;
            }

            ASNRead objdet = new ASNRead
            {
                asnheader = infoHdr,
                asnlistdetail = infodetail
            
            };

            
            return objdet;

           
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
