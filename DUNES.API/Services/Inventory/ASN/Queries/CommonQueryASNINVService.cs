using AutoMapper;
using DUNES.API.DTOs.B2B;
using DUNES.API.Repositories.Inventory.ASN.Queries;

using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
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
        private readonly IMapper _mapper;

        /// <summary>
        /// Dependency Injection
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        public CommonQueryASNINVService(ICommonQueryASNINVRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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


            //assig model data to dto data
            var objenc = _mapper.Map<ASNHdrDto>(info.asnheader);

            var listDetail = _mapper.Map<List<ASNItemDetailDto>>(info.asnlistdetail ?? new());



            ASNWm objreturn = new ASNWm
            {
                asnHdr = objenc,
                itemDetail = listDetail,
                
            };

            if (info.receivingHdr != null)
            {
                var objencrec = _mapper.Map<TzebB2bIrReceiptOutHdrDetItemInbConsReqsLogDto>(info.receivingHdr);

                objreturn.asnReceiptHdr = objencrec;

                
                if (info.receiveingListDetail != null && info.receiveingListDetail.Count > 0)
                {
                    var listDetailrec = _mapper.Map<List<TzebB2bIrReceiptLineItemTblItemInbConsReqsLogDto>>(info.receiveingListDetail);

                    objreturn.asnReceiptList = listDetailrec;
                }
            }

            return ApiResponseFactory.Ok(objreturn, "OK");

        }
       
    }
}
