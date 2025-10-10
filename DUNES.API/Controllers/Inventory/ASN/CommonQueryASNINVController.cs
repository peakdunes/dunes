using DUNES.API.Services.Inventory.ASN.Queries;
using DUNES.API.Services.Inventory.ASN.Transactions;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using DUNES.Shared.WiewModels.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.Controllers.Inventory.ASN
{

    /// <summary>
    /// Common Inventory queries (ASN - PickProcess
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CommonQueryASNINVController : BaseController
    {

        private readonly ICommonQueryASNINVService _service;
        private readonly ITransactionsASNINVService _transactionASNService;
        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="service"></param>
        /// <param name="transactionASNService"></param>
        public CommonQueryASNINVController(ICommonQueryASNINVService service, ITransactionsASNINVService transactionASNService)
        {
            _service = service;
            _transactionASNService = transactionASNService;
        }

        /// <summary>
        /// get all ASN information
        /// </summary>
        /// <param name="ShipmentNum"></param>
        /// <remarks>Returns header plus one or more detail lines.</remarks>
        /// <response code="200">Successful; returns data.</response>
        /// <response code="404">Not found if the delivery does not exist.</response>
        /// <returns>
        /// An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse{ASNDto}"/> 
        /// with the pick order header and details.
        /// </returns>
        [ProducesResponseType(typeof(ApiResponse<ASNWm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]

        [HttpGet("asn-info/{ShipmentNum}")]
        public async Task<IActionResult> GetASNAllInfo (string ShipmentNum, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetASNAllInfo(ShipmentNum, ct), ct);
        }


        /// <summary>
        /// Process ASN Transaction
        /// </summary>
        /// <param name="AsnId">ID Transaction NUmber</param>
        /// <param name="WmsTran">WMS Transaction (Header and Detail)</param>
        /// <param name="TrackingNumber">Tracking Number ASN Receiving</param>
        /// <param name="listdetail">ASN Bins distribution</param>
        /// <param name="ct">Cancelation TOken</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<ASNWm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [HttpGet("asn-process/{ShipmentNum}")]
        public async Task<IActionResult> ProcessASNTransaction(string AsnId, NewInventoryTransactionTm WmsTran, string TrackingNumber , List<BinsToLoadWm> listdetail, CancellationToken ct)
        {
            return await HandleApi(ct => _transactionASNService.CreateASNReceivingTransaction(AsnId, WmsTran, TrackingNumber, listdetail, ct), ct);
        }


    }
}
