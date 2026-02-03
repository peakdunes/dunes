using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Masters.Items;
using DUNES.Shared.DTOs.WMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.Items
{
    /// <summary>
    /// Items API Controller.
    /// 
    /// Provides CRUD and state management endpoints for inventory items.
    /// 
    /// IMPORTANT (STANDARD COMPANYID):
    /// - CompanyId is NEVER accepted from route, query, or body.
    /// - CompanyId is always obtained from the authenticated token
    ///   via BaseController (CurrentCompanyId).
    /// - This controller contains NO business logic.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/wms/masters/items")]
    public class ItemsWMSController : BaseController
    {
        private readonly IItemsWMSAPIService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsWMSController"/> class.
        /// </summary>
        /// <param name="service">
        /// Items service injected via dependency injection.
        /// </param>
        public ItemsWMSController(IItemsWMSAPIService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all items for the current company.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse containing the list of items.
        /// </returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetAllAsync(CurrentCompanyId, ct),
                ct);
        }

        /// <summary>
        /// Retrieves all active items for the current company.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse containing the list of active items.
        /// </returns>
        [HttpGet("GetActive")]
        public async Task<IActionResult> GetActive(CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetActiveAsync(CurrentCompanyId, ct),
                ct);
        }

        /// <summary>
        /// Retrieves an item by its identifier.
        /// </summary>
        /// <param name="id">Item identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse containing the item if found.
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
        /// Creates a new inventory item.
        /// </summary>
        /// <param name="dto">
        /// Item DTO to create.
        /// CompanyId will be enforced from the authenticated context.
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse indicating the result of the operation.
        /// </returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            [FromBody] WMSItemsDTO dto,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.CreateAsync(CurrentCompanyId, dto, ct),
                ct);
        }

        /// <summary>
        /// Updates an existing inventory item.
        /// </summary>
        /// <param name="id">Item identifier.</param>
        /// <param name="dto">
        /// Item DTO containing updated values.
        /// CompanyId cannot be modified.
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse indicating the result of the operation.
        /// </returns>
        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] WMSItemsDTO dto,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.UpdateAsync(CurrentCompanyId, id, dto, ct),
                ct);
        }

        /// <summary>
        /// Activates or deactivates an inventory item.
        /// </summary>
        /// <param name="id">Item identifier.</param>
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

        /// <summary>
        /// Checks if an item exists with the specified SKU.
        /// </summary>
        /// <param name="sku">Item SKU.</param>
        /// <param name="excludeId">
        /// Optional item identifier to exclude from the validation.
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse indicating whether the SKU exists.
        /// </returns>
        [HttpGet("ExistsBySku")]
        public async Task<IActionResult> ExistsBySku(
            [FromQuery] string sku,
            [FromQuery] int? excludeId,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.ExistsBySkuAsync(CurrentCompanyId, sku, excludeId, ct),
                ct);
        }

        /// <summary>
        /// Checks if an item exists with the specified Barcode.
        /// </summary>
        /// <param name="barcode">Item barcode.</param>
        /// <param name="excludeId">
        /// Optional item identifier to exclude from the validation.
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse indicating whether the barcode exists.
        /// </returns>
        [HttpGet("ExistsByBarcode")]
        public async Task<IActionResult> ExistsByBarcode(
            [FromQuery] string barcode,
            [FromQuery] int? excludeId,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.ExistsByBarcodeAsync(CurrentCompanyId, barcode, excludeId, ct),
                ct);
        }
    }
}
