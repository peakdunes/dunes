using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Masters.TransactionsType;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.TransactionTypes
{
    /// <summary>
    /// Transaction Types controller.
    ///
    /// This controller exposes endpoints for managing
    /// Transaction Types within the WMS Masters module.
    ///
    /// IMPORTANT (STANDARD COMPANYID):
    /// - CompanyId is resolved from the authenticated context
    ///   via BaseController.
    /// - CompanyId is NEVER received from route, query, or body.
    /// - All business logic is delegated to the Service layer.
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TransactionTypesWMSController : BaseController
    {
        private readonly ITransactionsTypeWMSAPIService _service;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TransactionTypesWMSController"/> class.
        /// </summary>
        /// <param name="service">
        /// Transaction Types service injected via DI.
        /// </param>
        public TransactionTypesWMSController(
            ITransactionsTypeWMSAPIService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all transaction types for the current company.
        /// </summary>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(ApiResponse<List<WMSTransactiontypesReadDTO>>), StatusCodes.Status200OK)]
        public Task<IActionResult> GetAll(CancellationToken ct)
            => HandleApi( ct =>  _service.GetAllAsync(CurrentCompanyId, ct), ct);

        /// <summary>
        /// Retrieves all active transaction types for the current company.
        /// </summary>
        [HttpGet("GetActive")]
        [ProducesResponseType(typeof(ApiResponse<List<WMSTransactiontypesReadDTO>>), StatusCodes.Status200OK)]
        public Task<IActionResult> GetActive(CancellationToken ct)
            => HandleApi(ct =>    _service.GetActiveAsync(CurrentCompanyId, ct), ct);

        /// <summary>
        /// Retrieves a transaction type by its identifier.
        /// </summary>
        /// <param name="id">Transaction type identifier</param>
        [HttpGet("GetById/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<WMSTransactiontypesReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public Task<IActionResult> GetById(
            int id,
            CancellationToken ct)
            => HandleApi(ct => _service.GetByIdAsync(CurrentCompanyId, id, ct), ct);

        /// <summary>
        /// Creates a new transaction type.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="ct"></param>
        /// <returns>Dto created</returns>
        [HttpPost("Create")]
        [ProducesResponseType(typeof(ApiResponse<WMSTransactiontypesCreateDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        public Task<IActionResult> Create(
            [FromBody] WMSTransactiontypesCreateDTO dto,
            CancellationToken ct)
            => HandleApi(ct =>  _service.CreateAsync(CurrentCompanyId, dto, ct), ct);

        /// <summary>
        /// Updates an existing transaction type.
        /// </summary>
        /// <param name="id">Transaction type identifier</param>
        /// <param name="dto">Transaction type update DTO</param>
        /// <param name="ct">cancelationToken</param>
        /// <returns></returns>
        [HttpPut("Update/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<WMSTransactionTypesUpdateDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        public Task<IActionResult> Update(
            int id,
            [FromBody] WMSTransactionTypesUpdateDTO dto,
            CancellationToken ct)
            => HandleApi( ct =>     _service.UpdateAsync(CurrentCompanyId, id, dto, ct), ct);

        /// <summary>
        /// Activates or deactivates a transaction type.
        /// </summary>
        /// <param name="id">Transaction type identifier</param>
        /// <param name="isActive">Activation flag</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPatch("SetActive/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public Task<IActionResult> SetActive(
            int id,
            [FromQuery] bool isActive,
            CancellationToken ct)
            => HandleApi(ct =>  _service.SetActiveAsync(CurrentCompanyId, id, isActive, ct), ct);
    }
}
