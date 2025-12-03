
using AutoMapper;
using DUNES.API.Models.B2b;
using DUNES.API.Models.Inventory.PickProcess;
using DUNES.API.ReadModels.B2B;
using DUNES.API.ReadModels.Inventory;
using DUNES.API.Repositories.Inventory.ASN.Queries;
using DUNES.API.Repositories.Inventory.PickProcess.Queries;

using DUNES.Shared.DTOs.B2B;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using DUNES.Shared.Utils.Reponse;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Services.Inventory.PickProcess.Queries
{

    /// <summary>
    /// all inventory pickprocess queries
    /// </summary>
    public class CommonQueryPickProcessINVService : ICommonQueryPickProcessINVService
    {


        private readonly ICommonQueryPickProcessINVRepository _repository;
        private readonly IMapper _mapper;
        /// <summary>
        /// Dependency Injection
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        public CommonQueryPickProcessINVService(ICommonQueryPickProcessINVRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// get all pick process from a start date
        /// </summary>
        /// <param name="dateSearch"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<PickProcessHdrDto>>> GetAllPickProcessByStartDate(DateTime dateSearch, CancellationToken ct)
        {
            

            var info = await _repository.GetAllPickProcessByStartDate(dateSearch, ct);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<PickProcessHdrDto>>(
                    $"Pick process was found since  ({dateSearch}).");
            }
            var listPick = _mapper.Map<List<PickProcessHdrDto>>(info ?? new());

            return ApiResponseFactory.Ok(listPick, "OK");

        }

        /// <summary>
        /// Displays the 4 tables associated with an Pick Process in Servtrack.
        /// _TOrderRepair_Hdr
        /// _TorderRepair_ItemsSerials_Receiving
        /// _TorderRepair_ItemsSerials_Shipping 
        /// _TOrderRepair_Items
        /// </summary>
        /// <param name="ConsignRequestId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<TorderRepairTm>> GetAllTablesOrderRepairCreatedByPickProcessAsync(string ConsignRequestId, CancellationToken ct)
        {
            TorderRepairTm objresponse = new TorderRepairTm();

            var info = await _repository.GetAllTablesOrderRepairCreatedByPickProcessAsync(ConsignRequestId, ct);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<TorderRepairTm>(
                    $"There is not ServTrack Tables for this Pick Process number ({ConsignRequestId}).");
            }

            TorderRepairHdrDto objenc = new TorderRepairHdrDto();

            objenc.RefNo = info.OrHdr.RefNo;
            objenc.DateCreated = info.OrHdr.DateCreated;
            objenc.TcustNo = info.OrHdr.TcustNo;
            objenc.CustRef = info.OrHdr.CustRef;
            objenc.CustName = info.OrHdr.CustName;
            objenc.ShipToAddr = info.OrHdr.ShipToAddr;
            objenc.ShipToAddr1 = info.OrHdr.ShipToAddr1;
            objenc.TcityId = info.OrHdr.TcityId;
            objenc.TstateId = info.OrHdr.TstateId;
            objenc.ZipCode = info.OrHdr.ZipCode;
            objenc.TstatusId = info.OrHdr.TstatusId;
            objenc.DateInserted = info.OrHdr.DateInserted;

            objresponse.repairHdr = objenc;

            if (info.ItemList != null)
            {
                foreach (var item in info.ItemList)
                {
                    TorderRepairItemsDto objitem = new TorderRepairItemsDto();

                    objitem.RefNo = item.RefNo;
                    objitem.PartNo = item.PartNo;
                    objitem.Qty = item.Qty;
                    objitem.CompanyPartNo = item.CompanyPartNo;

                    objresponse.ListItems.Add(objitem);
                }
            }

            if (info.ReceivingList != null)
            {
                foreach (var item in info.ReceivingList)
                {
                    TorderRepairItemsSerialsReceivingDto objitem = new TorderRepairItemsSerialsReceivingDto();

                    objitem.RefNo = item.RefNo; 
                    objitem.PartNo = item.PartNo;
                    objitem.DateReceived = item.DateReceived;
                    objitem.SerialInbound = item.SerialInbound;
                    objitem.SerialReceived = item.SerialReceived;
                    objitem.TstatusId = item.TstatusId;
                    objitem.RepairNo = item.RepairNo;
                    objitem.Qty = item.Qty;
                    objitem.QtyReceived = item.QtyReceived;
                    objitem.Id = item.Id;
                    objitem.ProjectName = item.ProjectName;
                    objresponse.ListItemsSerialsReceiving.Add(objitem);
                }
            }

            if (info.ShippingList != null)
            {
                foreach (var item in info.ShippingList)
                {
                    TorderRepairItemsSerialsShippingDto objitem = new TorderRepairItemsSerialsShippingDto();

                    objitem.RefNo = item.RefNo;
                    objitem.PartNo = item.PartNo;
                    objitem.DateShip = item.DateShip;
                    objitem.SerialShip = item.SerialShip;
                    objitem.TstatusId = item.TstatusId;
                    objitem.RepairNo = item.RepairNo;
                    objitem.Qty = item.Qty;
                    objitem.QtyShip = item.QtyShip;
                    objitem.ShipViaId = item.ShipViaId;
                    objitem.Id = item.Id;
                    objitem.TrackingNumber = item.TrackingNumber;
                    objitem.ShippingGroupId = item.ShippingGroupId;
                    objitem.DateTimeShip = item.DateTimeShip;
                    objitem.UserName = item.UserName;
                    objitem.DateTrackingNumber = item.DateTrackingNumber;

                    objresponse.ListItemsSerialsShipping.Add(objitem) ;
                }
            }
            return ApiResponseFactory.Ok(objresponse, "OK");
        }



     

        /// <summary>
        /// Get all Pick Process Information
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<PickProcessRequestDto>> GetPickProcessAllInfo(string DeliveryId, CancellationToken ct)
        {
            var info = await _repository.GetPickProcessAllInfo(DeliveryId, ct);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<PickProcessRequestDto>(
                    $"This Pick Process number ({DeliveryId}) doesn't exist in our system.");
            }


            PickProcessHdrDto objenc = new PickProcessHdrDto();

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
                objdet.RequestQuantity = Convert.ToInt32(item.RequestedQuantity);
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
