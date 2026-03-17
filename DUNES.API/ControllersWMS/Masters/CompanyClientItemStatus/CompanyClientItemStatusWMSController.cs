using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Masters.CompanyClientInventoryCategory;
using DUNES.API.ServicesWMS.Masters.CompanyClientItemStatus;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.CompanyClientItemStatus
{
    /// <summary>
    /// Manages client-level ItemStatus mappings (assign, update, activate/deactivate, delete)
    /// using tenant scope from token (CompanyId / CompanyClientId).
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/wms/masters/company-client/item-status")]
    public class CompanyClientItemStatusWMSController : BaseController
    {
        private readonly ICompanyClientItemStatusWMSAPIService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyClientItemStatusWMSController"/> class.
        /// </summary>
        /// <param name="service">Service for CompanyClientItemStatus business logic.</param>
        public CompanyClientItemStatusWMSController(ICompanyClientItemStatusWMSAPIService service)
        {
            _service = service;
        }

        /// <summary>
        /// Returns all enabled item status mappings for the current client.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of enabled item status mappings for the current client.</returns>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(ApiResponse<List<WMSCompanyClientItemStatusReadDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.GetAllAsync(CurrentCompanyId, CurrentCompanyClientId, ct), ct);
                       
        }

        /// <summary>
        /// Returns enabled item status mappings for the current client.
        /// This endpoint is explicit for enabled-only scenarios.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of enabled item status mappings for the current client.</returns>
        [HttpGet("GetEnabled")]
        [ProducesResponseType(typeof(ApiResponse<List<WMSCompanyClientItemStatusReadDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEnabled(CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.GetEnabledAsync(CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        }

        /// <summary>
        /// Returns a specific client item status mapping by its Id.
        /// </summary>
        /// <param name="id">Mapping Id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The requested mapping if found.</returns>
        [HttpGet("GetById/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<WMSCompanyClientItemStatusReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.GetByIdAsync(CurrentCompanyId, CurrentCompanyClientId, id, ct), ct);
        }

        /// <summary>
        /// Creates a new client mapping for a master item status.
        /// </summary>
        /// <param name="dto">Create DTO containing the master ItemStatusId and IsActive flag.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created client item status mapping.</returns>
        [HttpPost("Create")]
        [ProducesResponseType(typeof(ApiResponse<WMSCompanyClientItemStatusReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(
            [FromBody] WMSCompanyClientItemStatusCreateDTO dto,
            CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.CreateAsync(dto, CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        }

        /// <summary>
        /// Enables or disables an existing client mapping.
        /// </summary>
        /// <param name="id">Mapping Id.</param>
        /// <param name="dto">DTO containing the new IsActive value.</param>
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
            [FromBody] WMSCompanyClientItemStatusSetActiveDTO dto,
            CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.SetActiveAsync(id, dto.IsActive, CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        }

        /// <summary>
        /// Replaces the enabled set for the current client.
        /// Typical UI behavior: user selects the final list of allowed item statuses and clicks Save.
        /// </summary>
        /// <param name="dto">DTO containing the final list of enabled master item status ids.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Result of the bulk enablement update.</returns>
        [HttpPut("SetEnabledSet")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetEnabledSet(
            [FromBody] WMSCompanyClientItemStatusSetEnabledDTO dto,
            CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.SetEnabledSetAsync(dto.ItemStatusIds, CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        }

        /// <summary>
        /// Deletes a client mapping by Id.
        /// Important:
        /// - This deletes only the relationship.
        /// - It does not delete the master ItemStatus.
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
