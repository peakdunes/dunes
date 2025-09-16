
using DUNES.API.ReadModels.Inventory;
using DUNES.API.Repositories.Inventory.ASN.Queries;
using DUNES.API.Repositories.Inventory.PickProcess.Queries;
using DUNES.API.Utils.Responses;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<ApiResponse<PickProcessCallsReadDto>> GetPickProcessAllCalls(string DeliveryId)
        {

            List<InputCallsDto> listinputcalls = new List<InputCallsDto>();
            List<OutputCallsDto> listoutputcalls = new List<OutputCallsDto>();

            PickProcessCallsReadDto objdto = new PickProcessCallsReadDto();

            var info = await _repository.GetPickProcessAllCalls(DeliveryId);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<PickProcessCallsReadDto>(
                    $"There is not call information for this Pick Process number ({DeliveryId}).");
            }

          
            List<InputCallsDto> listInputCalls = new List<InputCallsDto>();

            List<OutputCallsDto> listOutputCalls = new List<OutputCallsDto>();

          
            if (info.inputCalls.Count > 0)
            {
                foreach (var call in info.inputCalls)
                {
                    InputCallsDto objin = new InputCallsDto();

                    objin.Id = call.Id;
                    objin.TransactionCode = call.TransactionCode;
                    objin.DateTimeInserted = call.DateTimeInserted;
                    objin.Error = call.Error;
                    objin.Processed = call.Processed;

                    listInputCalls.Add(objin);
                }
            }


           

            if (info.outputCalls.Count > 0)
            {
                foreach (var call in info.outputCalls)
                {
                    OutputCallsDto objout = new OutputCallsDto();

                 

                    objout.Id = call.Id;
                    objout.TypeOfCallId = call.TypeOfCallId;
                    objout.TypeOfCallDescription = call.callName;
                    objout.DateTimeInserted = call.DateTimeInserted;
                    objout.AckReceived = call.AckReceived;
                    objout.Result = call.Result;
                    objout.DateTimeSent = call.DateTimeSent;
                    objout.InProcess = call.InProcess;

                    listOutputCalls.Add(objout);
                }
            }

            objdto.inputCallsList = listInputCalls;
            objdto.outputCallsList = listOutputCalls;

            return ApiResponseFactory.Ok(objdto, "OK");
        }

        /// <summary>
        /// Get all Pick Process Information
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<PickProcessRequestDto>> GetPickProcessAllInfo(string DeliveryId)
        {
            var info = await _repository.GetPickProcessAllInfo(DeliveryId);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<PickProcessRequestDto>(
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

            PickProcessRequestDto objreturn = new PickProcessRequestDto
            {
                PickProcessHdr = objenc,
                ListItems = listDetail
            };


            return ApiResponseFactory.Ok(objreturn, "OK");
        }
    }
}
