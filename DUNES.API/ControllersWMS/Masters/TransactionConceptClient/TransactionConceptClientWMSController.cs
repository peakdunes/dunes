using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Masters.TransactionConceptClient;
using DUNES.Shared.DTOs;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.TransactionConceptClient
{
    /// <summary>
    /// Manages client-level Transaction Concept mappings (assign, update, activate/deactivate, delete)
    /// using tenant scope from token (CompanyId / CompanyClientId).
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/wms/masters/company-client/transaction-concept")]
    public class TransactionConceptClientWMSController : BaseController
    {
        private readonly ITransactionConceptClientWMSAPIService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionConceptClientWMSController"/> class.
        /// </summary>
        /// <param name="service">Service for TransactionConceptClient business logic.</param>
        public TransactionConceptClientWMSController(ITransactionConceptClientWMSAPIService service)
        {
            _service = service;
        }

        /// <summary>
        /// Returns all transaction concept mappings for the current client.
        /// Includes both active and inactive mappings.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of transaction concept mappings for the current client.</returns>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(ApiResponse<List<WMSTransactionConceptClientReadDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.GetAllAsync(CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        }

        /// <summary>
        /// Returns enabled transaction concept mappings for the current client.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of enabled transaction concept mappings for the current client.</returns>
        [HttpGet("GetEnabled")]
        [ProducesResponseType(typeof(ApiResponse<List<WMSTransactionConceptClientReadDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEnabled(CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.GetEnabledAsync(CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        }

        /// <summary>
        /// Returns a specific transaction concept mapping by Id.
        /// </summary>
        /// <param name="id">Mapping Id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The requested mapping if found.</returns>
        [HttpGet("GetById/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<WMSTransactionConceptClientReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.GetByIdAsync(id, CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        }

        /// <summary>
        /// Creates a new client mapping for a master transaction concept.
        /// </summary>
        /// <param name="dto">Create DTO containing the master TransactionConceptId and Active flag.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created client transaction concept mapping.</returns>
        [HttpPost("Create")]
        [ProducesResponseType(typeof(ApiResponse<WMSTransactionConceptClientReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(
            [FromBody] WMSTransactionConceptClientCreateDTO dto,
            CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.CreateAsync(dto, CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        }

        /// <summary>
        /// Updates an existing client transaction concept mapping.
        /// </summary>
        /// <param name="id">Mapping Id.</param>
        /// <param name="dto">Update DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The updated client transaction concept mapping.</returns>
        [HttpPut("Update/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<WMSTransactionConceptClientReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] WMSTransactionConceptClientUpdateDTO dto,
            CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.UpdateAsync(id, dto, CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        }

        /// <summary>
        /// Enables or disables an existing client transaction concept mapping.
        /// </summary>
        /// <param name="id">Mapping Id.</param>
        /// <param name="dto">DTO containing the new Active value.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Result of the activation update.</returns>
        [HttpPut("SetActive/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetActive(
            int id,
            [FromBody] WMSTransactionConceptClientSetActiveDTO dto,
            CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.SetActiveAsync(id, dto.Active, CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        }

        /// <summary>
        /// Deletes a client mapping by Id.
        /// Important:
        /// - This deletes only the relationship.
        /// - It does not delete the master TransactionConcept.
        /// </summary>
        /// <param name="id">Mapping Id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Result of the delete operation.</returns>
        [HttpDelete("Delete/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.DeleteAsync(id, CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        }
    }
}