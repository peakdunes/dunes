using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Masters.Items;
using DUNES.Shared.DTOs;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.Items
{
    /// <summary>
    /// Manages Items within WMS.
    /// Supports retrieval of company/master items, client-owned items, or both,
    /// depending on the item ownership mode resolved by the service layer.
    /// Uses tenant scope from token (CompanyId / CompanyClientId).
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/wms/masters/items")]
    public class ItemsWMSController : BaseController
    {
        private readonly IItemsWMSAPIService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsWMSController"/> class.
        /// </summary>
        /// <param name="service">Service for Items business logic.</param>
        public ItemsWMSController(IItemsWMSAPIService service)
        {
            _service = service;
        }

        /// <summary>
        /// Returns all items available for the current tenant scope
        /// according to the configured ownership mode.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of items visible to the current client context.</returns>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(ApiResponse<List<WMSItemsReadDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.GetAllAsync(CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        }

        /// <summary>
        /// Returns a specific item by Id within the current tenant scope
        /// according to the configured ownership mode.
        /// </summary>
        /// <param name="id">Item identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The requested item if found.</returns>
        [HttpGet("GetById/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<WMSItemsReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.GetByIdAsync(id, CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        }

        /// <summary>
        /// Creates a new item.
        /// Ownership and tenant scope are resolved in the service layer.
        /// </summary>
        /// <param name="dto">Create DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created item.</returns>
        [HttpPost("Create")]
        [ProducesResponseType(typeof(ApiResponse<WMSItemsReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(
            [FromBody] WMSItemsCreateDTO dto,
            CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.CreateAsync(dto, CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        }

        /// <summary>
        /// Updates an existing item by Id.
        /// </summary>
        /// <param name="id">Item identifier.</param>
        /// <param name="dto">Update DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The updated item.</returns>
        [HttpPut("Update/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<WMSItemsReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] WMSItemsUpdateDTO dto,
            CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.UpdateAsync(id, dto, CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        }

        /// <summary>
        /// Enables or disables an existing item.
        /// </summary>
        /// <param name="id">Item identifier.</param>
        /// <param name="dto">DTO containing the new active value.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Result of the active status update.</returns>
        [HttpPut("SetActive/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetActive(
            int id,
            [FromBody] WMSSetActiveDTO dto,
            CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.SetActiveAsync(id, dto.Active, CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        }

        /// <summary>
        /// Deletes an item by Id.
        /// </summary>
        /// <param name="id">Item identifier.</param>
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