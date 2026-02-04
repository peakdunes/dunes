using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Masters.TransactionConceptClient;
using DUNES.Shared.DTOs.WMS;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.TransactionConceptClient
{
    /// <summary>
    /// Manages Transaction Concept assignments per Company Client.
    /// </summary>
    [ApiController]
    [Route("api/wms/masters/transaction-concept-client")]
    public class TransactionConceptClientWMSController : BaseController
    {
        private readonly ITransactionConceptClientWMSAPIService _service;

        /// <summary>
        /// Initializes controller (DI).
        /// </summary>
        public TransactionConceptClientWMSController(
            ITransactionConceptClientWMSAPIService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets all Transaction Concepts assigned to a Company Client.
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
        /// Assigns a Transaction Concept to a Company Client.
        /// </summary>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            [FromBody] WMSTransactionConceptClientCreateDTO dto,
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
        /// Updates a Transaction Concept assignment.
        /// </summary>
        [HttpPut("Update")]
        public async Task<IActionResult> Update(
            [FromBody] WMSTransactionConceptClientUpdateDTO dto,
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
