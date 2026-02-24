using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Masters.TransactionConcepts;
using DUNES.Shared.DTOs.WMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.TransactionConcepts
{
    /// <summary>
    /// Transaction Concepts API Controller.
    /// 
    /// Provides CRUD and state management endpoints for Transaction Concepts.
    /// 
    /// IMPORTANT (STANDARD COMPANYID):
    /// - CompanyId is NEVER accepted from route, query, or body.
    /// - CompanyId is always obtained from the authenticated token
    ///   via BaseController (CurrentCompanyId).
    /// - This controller contains NO business logic.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/wms/masters/transaction-concepts")]
    public class TransactionConceptsWMSController : BaseController
    {
        private readonly ITransactionConceptsWMSAPIService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionConceptsWMSController"/> class.
        /// </summary>
        /// <param name="service">
        /// Transaction concepts service injected via dependency injection.
        /// </param>
        public TransactionConceptsWMSController(ITransactionConceptsWMSAPIService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all transaction concepts for the current company.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse containing the list of transaction concepts.</returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return await HandleApi(x => _service.GetAllAsync(CurrentCompanyId, x), ct);
        }

        /// <summary>
        /// Retrieves all active transaction concepts for the current company.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse containing the list of active transaction concepts.</returns>
        [HttpGet("GetActive")]
        public async Task<IActionResult> GetActive(CancellationToken ct)
        {
            return await HandleApi(x => _service.GetActiveAsync(CurrentCompanyId, x), ct);
        }

        /// <summary>
        /// Retrieves a transaction concept by its identifier.
        /// </summary>
        /// <param name="id">Transaction concept identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse containing the transaction concept if found.</returns>
        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            return await HandleApi(x => _service.GetByIdAsync(CurrentCompanyId, id, x), ct);
        }

        /// <summary>
        /// Creates a new transaction concept.
        /// </summary>
        /// <param name="model">Create DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse containing the newly created transaction concept.</returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            [FromBody] WMSTransactionconceptsCreateDTO model,
            CancellationToken ct)
        {
            return await HandleApi(x => _service.CreateAsync(model, CurrentCompanyId, x), ct);
        }

        /// <summary>
        /// Updates an existing transaction concept.
        /// </summary>
        /// <param name="id">Transaction concept identifier.</param>
        /// <param name="model">Update DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse containing the updated transaction concept.</returns>
        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] WMSTransactionconceptsUpdateDTO model,
            CancellationToken ct)
        {
            return await HandleApi(x => _service.UpdateAsync(id, model, CurrentCompanyId, x), ct);
        }

        /// <summary>
        /// Activates or deactivates a transaction concept.
        /// </summary>
        /// <param name="model">Set active DTO (Id + IsActive).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse containing the updated transaction concept.</returns>
        [HttpPatch("SetActive")]
        public async Task<IActionResult> SetActive(
            [FromBody] WMSTransactionconceptsSetActiveDTO model,
            CancellationToken ct)
        {
            return await HandleApi(x => _service.SetActiveAsync(model, CurrentCompanyId, x), ct);
        }

        /// <summary>
        /// Deletes a transaction concept from the master catalog.
        /// </summary>
        /// <param name="id">Transaction concept identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse indicating whether the record was deleted.</returns>
        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            return await HandleApi(x => _service.DeleteAsync(id, CurrentCompanyId, x), ct);
        }
    }
}