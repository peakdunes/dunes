using DUNES.API.DTOs.B2B;
using DUNES.API.Services.B2B.Common.Queries;
using DUNES.API.Utils.Responses;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.Controllers.B2B.Queries
{

    /// <summary>
    /// Controller for handle all B2B Queries
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
public class CommonQueryB2BController : BaseController
    {

        private readonly ICommonQueryB2BService _service;

        /// <summary>
        /// new controller instance
        /// </summary>
        /// <param name="service"></param>
        public CommonQueryB2BController(ICommonQueryB2BService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all information about one repair
        /// </summary>
        /// <param name="repairNumber"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("repair-info/{repairNumber}")]
        public async Task<IActionResult> GetRepairInfo(int repairNumber, CancellationToken ct)
        {

            return await HandleApi(ct => _service.GetRepairInfoAsync(repairNumber, ct), ct);

           
        }

        /// <summary>
        /// Get current area from sql function
        /// </summary>
        /// <param name="repairNumber"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("repair-GetCurrentArea/{repairNumber}")]
        public async Task<IActionResult> GetAreaByFunction(int repairNumber, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetAreaByFunction(repairNumber, ct), ct);

          
        }


        /// <summary>
        ///  Gets all RMA receiving info by serial number.
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("rma/receiving/{serialNumber}")]
        public async Task<IActionResult> GetRMAReceivingInfo(string serialNumber, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetRMAReceivingInfo(serialNumber, ct), ct);


        }

        /// <summary>
        /// Validates that the given RMA exists in all 4 required tables.
        /// </summary>
        /// <param name="refNo"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("rma/validate/{refNo}")]
        public async Task<IActionResult> ValidateRMAHasAllTables(int refNo, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetAllRMATablesCreatedAsync(refNo, ct), ct);

        }



        /// <summary>
        /// Displays the main 4 tables associated with an order in Servtrack.
        /// _TOrderRepair_Hdr
        /// _TorderRepair_ItemsSerials_Receiving
        /// _TorderRepair_ItemsSerials_Shipping 
        /// _TOrderRepair_Items
        /// </summary>
        /// <param name="refNo"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("order-repair/info/{refNo}")]
        public async Task<IActionResult> GetAllTablesOrderRepairCreatedAsync(int refNo, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetAllTablesOrderRepairCreatedAsync(refNo, ct), ct);

         
        }


        /// <summary>
        /// Get info about one repair when it's ready for receive
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<RepairReadyToReceiveDto>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // no hay datos
        [ProducesResponseType(StatusCodes.Status409Conflict)] // repair cancel, stopped o closed
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // error inesperado
        [HttpGet("repair-RepairReadyToReceive/{serialNumber}")]
        public async Task<IActionResult> GetRepairReadyToReceive(string serialNumber, CancellationToken ct )
        {
            return await HandleApi(ct => _service.GetRepairReadyToReceive(serialNumber, ct), ct);

         
        }

    }
}
