using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Masters.TransactionConceptClient;
using DUNES.Shared.DTOs.WMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.TransactionConceptClient
{
    /// <summary>
    /// Transaction Concept Client API Controller.
    ///
    /// Manages mappings between Transaction Concepts (master)
    /// and Company Clients.
    ///
    /// IMPORTANT (STANDARD COMPANYID):
    /// - CompanyId is NEVER accepted from route/query/body.
    /// - CompanyId is always taken from the authenticated token
    ///   through BaseController (CurrentCompanyId).
    /// - Controller contains NO business logic.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/wms/masters/transaction-concept-client")]
    public class TransactionConceptClientWMSController : BaseController
    {
        private readonly ITransactionConceptClientWMSAPIService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionConceptClientWMSController"/> class.
        /// </summary>
        /// <param name="service">Transaction Concept Client service.</param>
        public TransactionConceptClientWMSController(
            ITransactionConceptClientWMSAPIService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets all transaction concept mappings for a specific company client.
        /// </summary>
        /// <param name="companyClientId">Company client identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse with the list of mappings for the specified company client.
        /// </returns>
        [HttpGet("GetByClient/{companyClientId:int}")]
        public async Task<IActionResult> GetByClient(
            int companyClientId,
            CancellationToken ct)
        {
            return await HandleApi(
                token => _service.GetByClientAsync(CurrentCompanyId, companyClientId, token),
                ct);
        }

        /// <summary>
        /// Creates a new transaction concept mapping for a company client.
        /// </summary>
        /// <param name="dto">Create DTO for the mapping.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse with the created mapping.
        /// </returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            [FromBody] WMSTransactionConceptClientCreateDTO dto,
            CancellationToken ct)
        {
            return await HandleApi(
                token => _service.CreateAsync(CurrentCompanyId, dto, token),
                ct);
        }

        /// <summary>
        /// Activates or deactivates a mapping (patch style).
        /// </summary>
        /// <param name="companyClientId">Company client identifier.</param>
        /// <param name="id">Mapping identifier.</param>
        /// <param name="isActive">New active state.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse indicating whether the operation succeeded.
        /// </returns>
        [HttpPatch("SetActive/{companyClientId:int}/{id:int}")]
        public async Task<IActionResult> SetActive(
            int companyClientId,
            int id,
            [FromQuery] bool isActive,
            CancellationToken ct)
        {
            return await HandleApi(
                token => _service.SetActiveAsync(CurrentCompanyId, companyClientId, id, isActive, token),
                ct);
        }

        /// <summary>
        /// Deletes a mapping physically.
        /// </summary>
        /// <param name="companyClientId">Company client identifier.</param>
        /// <param name="id">Mapping identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse indicating whether the mapping was deleted.
        /// </returns>
        [HttpDelete("Delete/{companyClientId:int}/{id:int}")]
        public async Task<IActionResult> Delete(
            int companyClientId,
            int id,
            CancellationToken ct)
        {
            return await HandleApi(
                token => _service.DeleteAsync(CurrentCompanyId, companyClientId, id, token),
                ct);
        }
    }
}
