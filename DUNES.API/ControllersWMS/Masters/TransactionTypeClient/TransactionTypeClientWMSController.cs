using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Masters.TransactionTypeClient;
using DUNES.Shared.DTOs.WMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.TransactionTypeClient
{
    /// <summary>
    /// Manages Transaction Type assignments per Company Client.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/wms/masters/transaction-type-client")]
    public class TransactionTypeClientWMSController : BaseController
    {
        private readonly ITransactionTypeClientWMSAPIService _service;

        /// <summary>
        /// Initializes controller (DI).
        /// </summary>
        public TransactionTypeClientWMSController(
            ITransactionTypeClientWMSAPIService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets all Transaction Types assigned to a Company Client.
        /// </summary>
        /// <param name="companyClientId">Company Client identifier</param>
        [HttpGet("GetByClient/{companyClientId}")]
        public async Task<IActionResult> GetByClient(
            int companyClientId,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetByClientAsync(
                    CurrentCompanyId,
                    companyClientId,
                    ct),
                ct);
        }

        /// <summary>
        /// Assigns a Transaction Type to a Company Client.
        /// </summary>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            [FromBody] WMSTransactionTypeClientCreateDTO dto,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.CreateAsync(
                    CurrentCompanyId,
                    dto,
                    ct),
                ct);
        }

        /// <summary>
        /// Updates a Transaction Type assignment.
        /// </summary>
        [HttpPut("Update")]
        public async Task<IActionResult> Update(
            [FromBody] WMSTransactionTypeClientUpdateDTO dto,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.UpdateAsync(
                    CurrentCompanyId,
                    dto,
                    ct),
                ct);
        }
    }
}
