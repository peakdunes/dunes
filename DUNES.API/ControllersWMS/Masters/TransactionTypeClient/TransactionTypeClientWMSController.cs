using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Masters.TransactionTypeClient;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.TransactionTypeClient
{
    /// <summary>
    /// Manages client-level Transaction Type mappings (assign, update, activate/deactivate, delete)
    /// using tenant scope from token (CompanyId / CompanyClientId).
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/wms/masters/company-client/transaction-type")]
    public class TransactionTypeClientWMSController : BaseController
    {
        private readonly ITransactionTypeClientWMSAPIService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionTypeClientWMSController"/> class.
        /// </summary>
        /// <param name="service">Service for TransactionTypeClient business logic.</param>
        public TransactionTypeClientWMSController(ITransactionTypeClientWMSAPIService service)
        {
            _service = service;
        }

        /// <summary>
        /// Returns all transaction type mappings for the current client.
        /// Includes both active and inactive mappings.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of transaction type mappings for the current client.</returns>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(ApiResponse<List<WMSTransactionTypeClientReadDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.GetAllAsync(CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        }

        /// <summary>
        /// Returns enabled transaction type mappings for the current client.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of enabled transaction type mappings for the current client.</returns>
        [HttpGet("GetEnabled")]
        [ProducesResponseType(typeof(ApiResponse<List<WMSTransactionTypeClientReadDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEnabled(CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.GetEnabledAsync(CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        }

        /// <summary>
        /// Returns a specific transaction type mapping by Id.
        /// </summary>
        /// <param name="id">Mapping Id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The requested mapping if found.</returns>
        [HttpGet("GetById/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<WMSTransactionTypeClientReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.GetByIdAsync(id, CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        }

        /// <summary>
        /// Creates a new client mapping for a master transaction type.
        /// </summary>
        /// <param name="dto">Create DTO containing the master TransactionTypeId and Active flag.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created client transaction type mapping.</returns>
        [HttpPost("Create")]
        [ProducesResponseType(typeof(ApiResponse<WMSTransactionTypeClientReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(
            [FromBody] WMSTransactionTypeClientCreateDTO dto,
            CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.CreateAsync(dto, CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        }

        /// <summary>
        /// Updates an existing client transaction type mapping.
        /// </summary>
        /// <param name="id">Mapping Id.</param>
        /// <param name="dto">Update DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The updated client transaction type mapping.</returns>
        [HttpPut("Update/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<WMSTransactionTypeClientReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] WMSTransactionTypeClientUpdateDTO dto,
            CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.UpdateAsync(id, dto, CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        }

        /// <summary>
        /// Enables or disables an existing client transaction type mapping.
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
            [FromBody] WMSTransactionTypeClientSetActiveDTO dto,
            CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.SetActiveAsync(id, dto.Active, CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        }

        /// <summary>
        /// Deletes a client mapping by Id.
        /// Important:
        /// - This deletes only the relationship.
        /// - It does not delete the master TransactionType.
        /// </summary>
        /// <param name="id">Mapping Id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Result of the delete operation.</returns>
        [HttpDelete("Delete/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
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