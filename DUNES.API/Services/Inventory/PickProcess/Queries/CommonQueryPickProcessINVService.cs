using DUNES.API.Repositories.Inventory.ASN.Queries;
using DUNES.API.Repositories.Inventory.PickProcess.Queries;
using DUNES.API.Utils.Responses;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;

namespace DUNES.API.Services.Inventory.PickProcess.Queries
{

    /// <summary>
    /// all inventory pickprocess queries
    /// </summary>
    public class CommonQueryPickProcessINVService : ICommonQueryPickProcessINVService
    {


        private readonly ICommonQueryPickProcessINVRepository _repository;

        /// <summary>
        /// Dependency Injection
        /// </summary>
        /// <param name="repository"></param>
        public CommonQueryPickProcessINVService(ICommonQueryPickProcessINVRepository repository)
        {
            _repository = repository;
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
          

            foreach (var item in info.pickdetails)
            {
                PickProcessItemDetail objdet = new PickProcessItemDetail();

                objdet.Id = item.Id;
                objdet.idPickProcessHdr = item.Id;
                objdet.LindId = item.LineId;
                objdet.ItemNumber = item.ItemNumber;
                objdet.ItemDescription = item.ItemDescription;
                objdet.RequestQuantity =  Convert.ToInt32(item.RequestedQuantity);
                objdet.QuantityProcess = 0;
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
