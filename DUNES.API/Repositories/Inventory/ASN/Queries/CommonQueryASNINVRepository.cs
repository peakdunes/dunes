using DUNES.API.Data;
using DUNES.API.ReadModels.Inventory;
using DUNES.Shared.DTOs.Inventory;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace DUNES.API.Repositories.Inventory.ASN.Queries
{
    /// <summary>
    /// Common Inventory Queries
    /// </summary>
    public class CommonQueryASNINVRepository : ICommonQueryASNINVRepository
    {

        private readonly AppDbContext _context;
        private readonly appWmsDbContext _wmscontext;

        /// <summary>
        /// initialize dbcontext
        /// </summary>
        /// <param name="context"></param>
        /// <param name="wmscontext"></param>
        public CommonQueryASNINVRepository(AppDbContext context, appWmsDbContext wmscontext)
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
        public async Task<ASNRead?> GetASNAllInfo(string ShipmentNum)
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

            //we check if there is information about ASN receiving process

            var inforeceivingAsn = await _context.TzebB2bIrReceiptOutHdrDetItemInbConsReqsLog.FirstOrDefaultAsync(x => x.ShipmentNum == ShipmentNum);

            if (inforeceivingAsn != null)
            {
                objdet.receivingHdr = inforeceivingAsn;

                var inforeceivingAsnDetail = await _context.TzebB2bIrReceiptLineItemTblItemInbConsReqsLog.Where(x => x.IrReceiptOutHdrDetItemId == inforeceivingAsn.Id).ToListAsync();

                if (inforeceivingAsnDetail.Count > 0)
                {
                    objdet.receiveingListDetail = inforeceivingAsnDetail;
                }
            }
            
            
            return objdet;

           
        }
      
    }
}
