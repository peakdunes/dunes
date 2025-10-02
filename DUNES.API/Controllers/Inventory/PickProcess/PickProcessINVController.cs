using DUNES.API.Services.Inventory.PickProcess.Queries;
using DUNES.API.Services.Inventory.PickProcess.Transactions;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace DUNES.API.Controllers.Inventory.PickProcess
{

    /// <summary>
    /// All Inventory Pick Process queries
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]

    public class PickProcessINVController : BaseController
    {


        private readonly ICommonQueryPickProcessINVService _service;
        private readonly ITransactionsPickProcessINVService _transactionservice;

        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="service"></param>
        public PickProcessINVController(ICommonQueryPickProcessINVService service, ITransactionsPickProcessINVService transactionservice)
        {
            _service = service;
            _transactionservice = transactionservice;
        }


        /// <summary>Get Header and Detail Pick Process Information.</summary>
        /// <param name="DeliveryId">Delivery identifier to filter the pick process.</param>
        ///  <param name="ct"></param>
        /// <remarks>Returns header plus one or more detail lines.</remarks>
        /// <response code="200">Successful; returns data.</response>
        /// <response code="404">Not found if the delivery does not exist.</response>
        /// <returns>
        /// An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse{PickOrderDto}"/> 
        /// with the pick order header and details.
        /// </returns>
        [ProducesResponseType(typeof(ApiResponse<PickProcessRequestDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [HttpGet("pickprocess-info/{DeliveryId}")]
        public async Task<IActionResult> GetPickProcessAllInfo(string DeliveryId, CancellationToken ct)
        {
            return await HandleApi(async ct =>
            {

                return await _service.GetPickProcessAllInfo(DeliveryId, ct);
            }, ct);
        }
      


        /// <summary>
        ///  /// Displays the 4 tables associated with an Pick Process in Servtrack.
        /// _TOrderRepair_Hdr
        /// _TorderRepair_ItemsSerials_Receiving
        /// _TorderRepair_ItemsSerials_Shipping 
        /// _TOrderRepair_Items
        /// </summary>
        /// <param name="ConsignRequestId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<TorderRepairTm>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<object>))]
        [HttpGet("repair-info/{ConsignRequestId}")]
        public async Task<IActionResult> GetRepairInfo(string ConsignRequestId, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetAllTablesOrderRepairCreatedByPickProcessAsync(ConsignRequestId, ct), ct);
        }


        /// <summary>
        /// Create a complete repair order servtrak order from a pick process number
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<PickProcessRequestDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [HttpPost("order-repair-create-from-delivery/{DeliveryId}")]
        public async Task<IActionResult> CreateServTrackOrderFromPickProcess(string DeliveryId)
        {
            var response = await _transactionservice.CreateServTrackOrderFromPickProcess(DeliveryId);

            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Create WMS Pick Process Transaction
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <param name="objInvTransaction"></param>
        /// <param name="lpnid"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<PickProcessResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        //[HttpPost("create-pickprocess-transaction/{DeliveryId}/{objInvTransaction}/{lpnid}")]
        //public async Task<IActionResult> CreatePickProccessTransaction(string DeliveryId, NewInventoryTransactionTm objInvTransaction, string lpnid, CancellationToken ct)
        //{
        //    var response = await _transactionservice.CreatePickProccessTransaction(DeliveryId, objInvTransaction, lpnid, ct);

        //    return StatusCode(response.StatusCode, response);
        //}
        [HttpPost("create-pickprocess-transaction/{DeliveryId}/{lpnid}")]
        public async Task<IActionResult> CreatePickProccessTransaction(string DeliveryId, [FromBody] NewInventoryTransactionTm objInvTransaction, string lpnid, CancellationToken ct)
        {
            var response = await _transactionservice.CreatePickProccessTransaction(DeliveryId, objInvTransaction, lpnid, ct);
            return StatusCode(response.StatusCode, response);
        }

    }
}
