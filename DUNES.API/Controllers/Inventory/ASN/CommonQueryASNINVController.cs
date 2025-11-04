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
        /// <param name="ASNnumber"></param>
        /// <param name="ct"></param>
        /// <remarks>Returns header plus one or more detail lines.</remarks>
        /// <response code="200">Successful; returns data.</response>
        /// <response code="404">Not found if the delivery does not exist.</response>
        /// <returns>
        /// An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse{ASNDto}"/> 
        /// with the pick order header and details.
        /// </returns>
        [ProducesResponseType(typeof(ApiResponse<ASNWm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]

        [HttpGet("asn-info/{ASNnumber}")]
        public async Task<IActionResult> GetASNAllInfo (string ASNnumber, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetASNAllInfo(ASNnumber, ct), ct);
        }


        /// <summary>
        /// Process ASN Transaction
        /// </summary>
        /// <param name="AsnId">ID Transaction NUmber</param>
        /// <param name="AsnInfo">WMS Transaction (Header and Detail)</param>
        /// <param name="TrackingNumber">Tracking Number ASN Receiving</param>
        /// <param name="ct">Cancelation TOken</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<ASNWm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [HttpPost("asn-process/{AsnId}/{TrackingNumber}")]
        public async Task<IActionResult> ProcessASNTransaction(string AsnId, [FromBody] ProcessAsnRequestTm AsnInfo, string TrackingNumber , CancellationToken ct)
        {
            return await HandleApi(ct => _transactionASNService.CreateASNReceivingTransaction(AsnId, AsnInfo.wmsInfo, TrackingNumber, AsnInfo.listdetail, ct), ct);
        }


    }
}
