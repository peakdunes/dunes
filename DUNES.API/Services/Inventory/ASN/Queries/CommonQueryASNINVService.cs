using DUNES.API.DTOs.B2B;
using DUNES.API.Repositories.Inventory.ASN.Queries;
using DUNES.API.Utils.Responses;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.WiewModels.Inventory;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DUNES.API.Services.Inventory.ASN.Queries
{


    /// <summary>
    /// Common all Inventory Queries
    /// </summary>
    public class CommonQueryASNINVService : ICommonQueryASNINVService
    {

       
        private readonly ICommonQueryASNINVRepository _repository;

        /// <summary>
        /// Dependency Injection
        /// </summary>
        /// <param name="repository"></param>
        public CommonQueryASNINVService(ICommonQueryASNINVRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get all ASN information
        /// </summary>
        /// <param name="ShipmentNum"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// 

        public async Task<ApiResponse<ASNWm>> GetASNAllInfo(string ShipmentNum, CancellationToken ct)
        {

            var info = await _repository.GetASNAllInfo(ShipmentNum);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<ASNWm>(
                    $"This ASN number ({ShipmentNum}) doesn't exist in our system.");
            }


            ASNHdr objenc = new ASNHdr();

            objenc.Id = info.asnheader.Id;
            objenc.ConsignRequestID = info.asnheader.ConsignRequestId;
            objenc.BatchId = info.asnheader.BatchId;
            objenc.ShipmentNum = info.asnheader.ShipmentNum;
            objenc.ShipToLocationId = Convert.ToInt32(info.asnheader.ShipToLocationId);
            objenc.DateTimeInserted = info.asnheader.DateTimeInserted;
            objenc.Processed = info.asnheader.Processed;

            List<ASNItemDetail> listDetail = new List<ASNItemDetail>();
            

            foreach (var item in info.asnlistdetail)
            {

                ASNItemDetail objdet = new ASNItemDetail();

                objdet.Id = item.Id;
                objdet.ItemId = Convert.ToInt32(item.InventoryItemId);
                objdet.ItemNumber = item.ItemNumber;
                objdet.LineId = Convert.ToInt32(item.LineNum);
                objdet.QuantityShipped = Convert.ToInt32(item.QuantityShipped);
                objdet.ItemDescription = item.ItemDescription;
                objdet.QuantityReceived = Convert.ToInt32(item.QuantityReceived);
                objdet.DateTimeInserted = item.DateTimeInserted;
                objdet.Attributte2 = item.Attribute2;
                objdet.QuantityPending = 0;// objdet.QuantityShipped - objdet.QuantityReceived;
                objdet.thereisdistribution = false;
                objdet.processed = item.Processed;
             

                listDetail.Add(objdet);

            }

            ASNWm objreturn = new ASNWm
            {
                asnHdr = objenc,
                itemDetail = listDetail
            };

           
            return ApiResponseFactory.Ok(objreturn, "OK");

        }
       
    }
}
