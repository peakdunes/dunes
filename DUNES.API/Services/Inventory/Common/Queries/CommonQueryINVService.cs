using DUNES.API.DTOs.B2B;
using DUNES.API.Repositories.Inventory.Common.Queries;
using DUNES.API.Utils.Responses;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DUNES.API.Services.Inventory.Common.Queries
{


    /// <summary>
    /// Common all Inventory Queries
    /// </summary>
    public class CommonQueryINVService : ICommonQueryINVService
    {


        private readonly ICommonQueryINVRepository _repository;

        /// <summary>
        /// Dependency Injection
        /// </summary>
        /// <param name="repository"></param>
        public CommonQueryINVService(ICommonQueryINVRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get all ASN information
        /// </summary>
        /// <param name="ShipmentNum"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// 

        public async Task<ApiResponse<ASNDto>> GetASNAllInfo(string ShipmentNum)
        {

            var info = await _repository.GetASNAllInfo(ShipmentNum);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<ASNDto>(
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
            ASNItemDetail objdet = new ASNItemDetail();

            foreach (var item in info.asnlistdetail)
            {

                objdet.Id = item.Id;
                objdet.ADNHdrId = Convert.ToInt32(item.AsnOutHdrDetItemId);
                objdet.ItemNumber = item.ItemNumber;
                objdet.LineId = Convert.ToInt32(item.LineNum);
                objdet.QuantityShipped = Convert.ToInt32(item.QuantityShipped);
                objdet.ItemDescription = item.ItemDescription;
                objdet.QuantityReceived = Convert.ToInt32(item.QuantityReceived);
                objdet.DateTimeInserted = item.DateTimeInserted;

                listDetail.Add(objdet);

            }

            ASNDto objreturn = new ASNDto
            {
                asnHdr = objenc,
                itemDetail = listDetail
            };

           
            return ApiResponseFactory.Ok(objreturn, "OK");

        }
        /// <summary>
        /// Get all Pick Process Information
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<PickProcessDto>> GetPickProcessAllInfo(string DeliveryId)
        {
            var info = await _repository.GetPickProcessAllInfo(DeliveryId);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<PickProcessDto>(
                    $"This Pick Process number ({DeliveryId}) doesn't exist in our system.");
            }


            PickProcessHdr objenc = new PickProcessHdr();

            objenc.Id = info.pickHdr.Id;
            objenc.ConsignRequestID = info.pickHdr.ConsignRequestId;
            objenc.Batchid = info.pickHdr.BatchId;
            objenc.HeaderId = info.pickHdr.HeaderId;
            objenc.DeliveryId = info.pickHdr.DeliveryId;
            objenc.Address1 = info.pickHdr.ShipToAddress1;
            objenc.Address2 = info.pickHdr.ShipToAddress2;
            objenc.Address3 = info.pickHdr.ShipToAddress3;
            objenc.Address4 = info.pickHdr.ShipToAddress4;
            objenc.Country = info.pickHdr.ShipToCountry;
            objenc.City = info.pickHdr.ShipToCity;
            objenc.State = info.pickHdr.ShipToState;
            objenc.ZipCode = info.pickHdr.ShipToPostalCode;
            objenc.DateCreated = info.pickHdr.DateTimeInserted;
            objenc.Carrier = info.pickHdr.Carrier;
            objenc.ShipMethod = info.pickHdr.ShipMethod;
            objenc.DateTimeProcessed = Convert.ToDateTime(info.pickHdr.DateTimeProcessed);
            objenc.OutConsReqsId = Convert.ToInt32(info.pickHdr.OutConsReqsId);
            objenc.DateTimeConfirmed = Convert.ToDateTime(info.pickHdr.DateTimeConfirmed);
            objenc.DateTimeOnlineOrders = Convert.ToDateTime(info.pickHdr.DateTimeOnlineOrders);
            objenc.DateTimeError = Convert.ToDateTime(info.pickHdr.DateTimeError);
            objenc.ErrorMsg = info.pickHdr.ErrorMsg;
            objenc.OnlineOrdersErrorMsg = info.pickHdr.OnlineOrdersErrorMsg;
            objenc.DateTimeOnlineOrdersError = Convert.ToDateTime(info.pickHdr.DateTimeOnlineOrdersError);
            objenc.ShipOutConsReqsId = Convert.ToInt32(info.pickHdr.ShipOutConsReqsId);
            objenc.ShipDateTimeConfirmed = Convert.ToDateTime(info.pickHdr.ShipDateTimeConfirmed);
            objenc.ShipErrorMsg = info.pickHdr.ShipErrorMsg;
            objenc.ShipDateTimeError = Convert.ToDateTime(info.pickHdr.ShipDateTimeError);

            List<PickProcessItemDetail> listDetail = new List<PickProcessItemDetail>();
            PickProcessItemDetail objdet = new PickProcessItemDetail();

            foreach (var item in info.pickdetails)
            {

                objdet.Id = item.Id;
                objdet.idPickProcessHdr = item.Id;
                objdet.LindId = item.LineId;
                objdet.ItemNumber = item.ItemNumber;
                objdet.ItemDescription = item.ItemDescription;
                objdet.RequestQuantity = item.RequestedQuantity;
                objdet.Frm3plLocatorStatus = item.Frm3plLocatorStatus;
                objdet.PickLPN = item.PickLpn;
                objdet.DateTimeInserted = item.DateTimeInserted;
                objdet.QtyOnHand = item.QtyOnHand;

                listDetail.Add(objdet);

            }

            PickProcessDto objreturn = new PickProcessDto
            {
                PickProcessHdr = objenc,
                ListItems = listDetail
            };


            return ApiResponseFactory.Ok(objreturn, "OK");
        }
    }
}
