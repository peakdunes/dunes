using APIZEBRA.Services.B2B.Common.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIZEBRA.Controllers.B2B.Queries
{

    /// <summary>
    /// Controller for handle all B2B Queries
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
public class CommonQueryController : ControllerBase
    {

        private readonly ICommonQueryService _service;

        /// <summary>
        /// new controller instance
        /// </summary>
        /// <param name="service"></param>
        public CommonQueryController(ICommonQueryService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all information about one repair
        /// </summary>
        /// <param name="repairNumber"></param>
        /// <returns></returns>
        [HttpGet("repair-info/{repairNumber}")]
        public async Task<IActionResult> GetRepairInfo(int repairNumber)
        {
            var response = await _service.GetRepairInfoAsync(repairNumber);

            // devuelves el ApiResponse tal cual viene del servicio
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Get current area from sql function
        /// </summary>
        /// <param name="repairNumber"></param>
        /// <returns></returns>
        [HttpGet("repair-GetCurrentArea/{repairNumber}")]
        public async Task<IActionResult> GetAreaByFunction(int repairNumber)
        {
            var response = await _service.GetAreaByFunction(repairNumber);

            // devuelves el ApiResponse tal cual viene del servicio
            return StatusCode(response.StatusCode, response);
        }


        /// <summary>
        /// Gets all RMA receiving info by serial number.
        /// </summary>
        [HttpGet("rma/receiving/{serialNumber}")]
        public async Task<IActionResult> GetRMAReceivingInfo(string serialNumber)
        {
            var response = await _service.GetRMAReceivingInfo(serialNumber);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Validates that the given RMA exists in all 4 required tables.
        /// </summary>
        [HttpGet("rma/validate/{refNo}")]
        public async Task<IActionResult> ValidateRMAHasAllTables(int refNo)
        {
            var response = await _service.GetAllRMATablesCreatedAsync(refNo);
            return StatusCode(response.StatusCode, response);
        }


        /// <summary>
        /// Get info about one repair when it's ready for receive
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        [HttpGet("repair-RepairReadyToReceive/{serialNumber}")]
        public async Task<IActionResult> GetAreaByFunction(string serialNumber)
        {
            var response = await _service.GetRepairReadyToReceive(serialNumber);

            // devuelves el ApiResponse tal cual viene del servicio
            return StatusCode(response.StatusCode, response);
        }
    }
}
