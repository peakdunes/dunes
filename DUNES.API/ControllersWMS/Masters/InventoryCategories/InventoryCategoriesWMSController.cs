using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Masters.InventoryCategories;
using DUNES.Shared.DTOs.WMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.InventoryCategories
{
    /// <summary>
    /// Inventory Categories API Controller.
    /// 
    /// Provides CRUD and state management endpoints for inventory categories.
    /// 
    /// IMPORTANT (STANDARD COMPANYID):
    /// - CompanyId is NEVER accepted from route, query, or body.
    /// - CompanyId is always obtained from the authenticated token
    ///   via BaseController (CurrentCompanyId).
    /// - This controller contains NO business logic.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/wms/masters/inventory-categories")]
    public class InventoryCategoriesWMSController : BaseController
    {
        private readonly IInventoryCategoriesWMSAPIService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryCategoriesWMSController"/> class.
        /// </summary>
        /// <param name="service">
        /// Inventory categories service injected via dependency injection.
        /// </param>
        public InventoryCategoriesWMSController(
            IInventoryCategoriesWMSAPIService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all inventory categories for the current company.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse containing the list of inventory categories.
        /// </returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetAllAsync(CurrentCompanyId, ct),
                ct);
        }

        /// <summary>
        /// Retrieves an inventory category by its identifier.
        /// </summary>
        /// <param name="id">Inventory category identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse containing the inventory category if found.
        /// </returns>
        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(
            int id,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetByIdAsync(CurrentCompanyId, id, ct),
                ct);
        }

        /// <summary>
        /// Creates a new inventory category.
        /// </summary>
        /// <param name="dto">
        /// Inventory category DTO to create.
        /// CompanyId will be enforced from the authenticated context.
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse indicating the result of the operation.
        /// </returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            [FromBody] WMSInventoryCategoriesDTO dto,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.CreateAsync(CurrentCompanyId, dto, ct),
                ct);
        }

        /// <summary>
        /// Updates an existing inventory category.
        /// </summary>
        /// <param name="id">Inventory category identifier.</param>
        /// <param name="dto">
        /// Inventory category DTO containing updated values.
        /// CompanyId cannot be modified.
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse indicating the result of the operation.
        /// </returns>
        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] WMSInventoryCategoriesDTO dto,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.UpdateAsync(CurrentCompanyId, id, dto, ct),
                ct);
        }

        /// <summary>
        /// Activates or deactivates an inventory category.
        /// </summary>
        /// <param name="id">Inventory category identifier.</param>
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
            return await HandleApi(
                ct => _service.SetActiveAsync(CurrentCompanyId, id, isActive, ct),
                ct);
        }
    }
}
