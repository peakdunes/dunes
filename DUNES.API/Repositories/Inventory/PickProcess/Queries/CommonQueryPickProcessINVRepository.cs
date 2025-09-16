using DUNES.API.Data;
using DUNES.API.Models.Inventory;
using DUNES.API.ReadModels.Inventory;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

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
        /// Gell all (input, output) calls for an pick process
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <returns></returns>
        public async Task<PickProcessCallsRead> GetPickProcessAllCalls(string DeliveryId)
        {

            PickProcessCallsRead pickcalls = new PickProcessCallsRead();

            var infodelivery = await _context.TzebB2bPSoWoHdrTblItemInbConsReqsLog.FirstOrDefaultAsync(x => x.DeliveryId == DeliveryId);

            if (infodelivery == null)
            {
                return null;
            }

            List<TzebB2bInbConsReqs> listInputCalls = new List<TzebB2bInbConsReqs>();

            List<TzebB2bOutConsReqs> listOutputCalls = new List<TzebB2bOutConsReqs>();

            var listcallsIn = await _context.TzebB2bInbConsReqs.Where(x => x.Id == infodelivery.ConsignRequestId).ToListAsync();

            if (listcallsIn.Any())
            {
                listInputCalls = listcallsIn;
            }

            var listcallsOut = await _context.TzebB2bOutConsReqs
               .Where(x => x.Id == infodelivery.OutConsReqsId || x.Id == infodelivery.ShipOutConsReqsId).ToListAsync();

            if (listcallsOut.Any())
            {
                listOutputCalls = listcallsOut;
            }

            foreach (var info in listOutputCalls)
            {

                var infcalldet = await _context.TzebB2bConsignmentCallsType.FirstOrDefaultAsync(x => x.Id == info.TypeOfCallId);
                if (infcalldet != null)
                {
                    info.callName = infcalldet.Name;
                }



            }

            pickcalls.inputCalls = listInputCalls;
            pickcalls.outputCalls = listOutputCalls;

            return pickcalls;
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
