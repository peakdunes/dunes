using DUNES.API.Services.WebService.Queries;
using DUNES.API.Services.WebService.Transactions;
using DUNES.Shared.DTOs.WebService;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.Controllers.WebService
{
    /// <summary>
    /// web service controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    
    public class WebServiceController : BaseController
    {

        private readonly ICommonQueryWebServiceService _commonQueryWebServiceService;
        private readonly ITransactionsWebServiceService _transactionsWebServiceService;


        /// <summary>
        /// dependency injection
        /// </summary>;
        /// <param name="commonQueryWebServiceService"></param>
        /// <param name="transactionsWebServiceService"></param>
        public WebServiceController(ICommonQueryWebServiceService commonQueryWebServiceService,
            ITransactionsWebServiceService transactionsWebServiceService)
        {
            _commonQueryWebServiceService = commonQueryWebServiceService;
            _transactionsWebServiceService = transactionsWebServiceService;

        }

        /// <summary>
        /// Get all summary call by day-hour
        /// </summary>
        /// <param name="dateRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("calls-by-day-hour/{dateRequest:datetime}")]
        public async Task<IActionResult> GetCallsByDayHour([FromRoute] DateTime dateRequest, CancellationToken ct)
        {
            return await HandleApi(ct => _commonQueryWebServiceService.GetHourlyTransactions(dateRequest, ct), ct);
        }


        /// <summary>
        /// Add or Update record in summary table
        /// </summary>
        /// <param name="inforecord"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("hourly")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
        
        public async Task<IActionResult> UpsertHourly([FromBody] MvcWebServiceHourlySummaryDto inforecord, CancellationToken ct)
        {
            var resp = await _transactionsWebServiceService.UpsertHourlyAsync(inforecord, ct);

            // Si usas ApiResponseFactory, normalmente basta con devolver 200 siempre
            // y que el cliente lea resp.Success; si prefieres status dinámico:
            return resp.Success ? Ok(resp) : BadRequest(resp);
        }
    }
}
