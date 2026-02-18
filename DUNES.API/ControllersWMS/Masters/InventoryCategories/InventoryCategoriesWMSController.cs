using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Masters.InventoryCategories;
using DUNES.Shared.DTOs.WMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.InventoryCategories
{
    /// <summary>
    /// Inventory Categories API Controller (WMS / Masters).
    ///
    /// Provides endpoints to manage Inventory Categories for the current tenant.
    ///
    /// IMPORTANT (STANDARD COMPANYID):
    /// - CompanyId is NEVER accepted from route, query, or body.
    /// - CompanyId is always obtained from the authenticated token via <see cref="BaseController.CurrentCompanyId"/>.
    /// - This controller contains NO business logic; it only delegates to the Service layer.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/wms/masters/inventory-categories")]
    // [ApiExplorerSettings(GroupName = "WMS")] // Optional: Swagger grouping
    public class InventoryCategoriesWMSController : BaseController
    {
        private readonly IInventoryCategoriesWMSAPIService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryCategoriesWMSController"/>.
        /// </summary>
        /// <param name="service">Inventory categories service.</param>
        public InventoryCategoriesWMSController(IInventoryCategoriesWMSAPIService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all inventory categories for the current company (tenant).
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse with a list of inventory categories (can be empty).</returns>
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetAllAsync(CurrentCompanyId, ct),
                ct);
        }

        /// <summary>
        /// Retrieves a single inventory category by its identifier for the current company (tenant).
        /// </summary>
        /// <param name="id">Inventory category identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse with the category if found; otherwise NotFound.</returns>
        [HttpGet("GetById/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetByIdAsync(CurrentCompanyId, id, ct),
                ct);
        }

        /// <summary>
        /// Obtain category by name
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="name"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("GetByName/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByName(int companyId, string name, CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.ExistsByNameAsync(CurrentCompanyId, name,0, ct),
                ct);
        }


        /// <summary>
        /// Creates a new inventory category for the current company (tenant).
        /// </summary>
        /// <param name="dto">Category creation DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse indicating success or validation/duplicate errors.</returns>
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] WMSInventorycategoriesCreateDTO dto, CancellationToken ct)
        {
            // If your HandleApi already validates ModelState, you can remove this block.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return await HandleApi(
                ct => _service.CreateAsync(CurrentCompanyId, dto, ct),
                ct);
        }

        /// <summary>
        /// Updates an existing inventory category for the current company (tenant).
        /// </summary>
        /// <param name="id">Inventory category identifier (route). This is authoritative.</param>
        /// <param name="dto">Category update DTO (body).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse indicating success or validation/not-found/duplicate errors.</returns>
        [HttpPut("Update/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int id, [FromBody] WMSInventorycategoriesUpdateDTO dto, CancellationToken ct)
        {
            // If your HandleApi already validates ModelState, you can remove this block.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return await HandleApi(
                ct => _service.UpdateAsync(CurrentCompanyId, id, dto, ct),
                ct);
        }

        /// <summary>
        /// Activates or deactivates an inventory category for the current company (tenant).
        /// </summary>
        /// <param name="id">Inventory category identifier.</param>
        /// <param name="isActive">True to activate; false to deactivate (querystring).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse indicating success or not found.</returns>
        [HttpPatch("SetActive/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetActive(int id, [FromQuery] bool isActive, CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.SetActiveAsync(CurrentCompanyId, id, isActive, ct),
                ct);
        }
    }
}
