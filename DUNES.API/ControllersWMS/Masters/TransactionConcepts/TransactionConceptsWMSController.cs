using DUNES.API.Controllers;
using DUNES.API.ModelsWMS.Masters;
using DUNES.API.ServicesWMS.Masters.TransactionConcepts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public TransactionConceptsWMSController(
            ITransactionConceptsWMSAPIService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all transaction concepts for the current company.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse containing the list of transaction concepts.
        /// </returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetAllAsync(CurrentCompanyId, ct), ct);
        }

        /// <summary>
        /// Retrieves all active transaction concepts for the current company.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse containing the list of active transaction concepts.
        /// </returns>
        [HttpGet("GetActive")]
        public async Task<IActionResult> GetActive(CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetActiveAsync(CurrentCompanyId, ct), ct);
        }

        /// <summary>
        /// Retrieves a transaction concept by its identifier.
        /// </summary>
        /// <param name="id">Transaction concept identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse containing the transaction concept if found.
        /// </returns>
        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(
            int id,
            CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetByIdAsync(CurrentCompanyId, id, ct), ct);
        }

        /// <summary>
        /// Creates a new transaction concept.
        /// </summary>
        /// <param name="model">
        /// Transaction concept entity to create.
        /// CompanyId will be enforced from the authenticated context.
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse containing the newly created transaction concept.
        /// </returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            [FromBody] Transactionconcepts model,
            CancellationToken ct)
        {
            return await HandleApi(ct => _service.CreateAsync(CurrentCompanyId, model, ct), ct);

           

        }

        /// <summary>
        /// Updates an existing transaction concept.
        /// </summary>
        /// <param name="id">Transaction concept identifier.</param>
        /// <param name="model">
        /// Transaction concept entity containing updated values.
        /// CompanyId cannot be modified.
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse containing the updated transaction concept.
        /// </returns>
        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] Transactionconcepts model,
            CancellationToken ct)
        {
            return await HandleApi(ct => _service.UpdateAsync(CurrentCompanyId, id, model, ct), ct);
        }

        /// <summary>
        /// Activates or deactivates a transaction concept.
        /// </summary>
        /// <param name="id">Transaction concept identifier.</param>
        /// <param name="isActive">Active state to apply.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse indicating the result of the operation.
        /// </returns>
        [HttpPatch("SetActive/{id:int}")]
        public async Task<IActionResult> SetActive(
            int id,
            [FromQuery] bool isActive,
            CancellationToken ct)
        {
            return await HandleApi(ct => _service.SetActiveAsync(CurrentCompanyId, id, isActive, ct), ct);
        }
    }
}
