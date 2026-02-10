using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Masters.InventoryTypes;
using DUNES.Shared.DTOs.WMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.InventoryTypes
{
    /// <summary>
    /// Inventory Types API Controller (WMS / Masters).
    ///
    /// Provides CRUD and state management endpoints for inventory types.
    ///
    /// IMPORTANT (STANDARD COMPANYID):
    /// - CompanyId is NEVER accepted from route, query, or body.
    /// - CompanyId is always obtained from the authenticated token via BaseController (CurrentCompanyId).
    /// - This controller contains NO business logic; it only delegates to the Service layer.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/wms/masters/inventory-types")]
    public class InventoryTypesWMSController : BaseController
    {
        private readonly IInventoryTypesWMSAPIService _service;

        /// <summary>
        /// Initializes a new instance of <see cref="InventoryTypesWMSController"/>.
        /// </summary>
        /// <param name="service">Inventory types service.</param>
        public InventoryTypesWMSController(IInventoryTypesWMSAPIService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all inventory types for the current company (tenant).
        /// </summary>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetAllAsync(CurrentCompanyId, ct),
                ct);
        }

        /// <summary>
        /// Retrieves active inventory types for the current company (tenant).
        /// </summary>
        [HttpGet("GetActive")]
        public async Task<IActionResult> GetActive(CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetActiveAsync(CurrentCompanyId, ct),
                ct);
        }

        /// <summary>
        /// Retrieves an inventory type by its identifier.
        /// </summary>
        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetByIdAsync(CurrentCompanyId, id, ct),
                ct);
        }

        /// <summary>
        /// Creates a new inventory type.
        /// </summary>
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] WMSInventoryTypesCreateDTO dto, CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.CreateAsync(CurrentCompanyId, dto, ct),
                ct);
        }

        /// <summary>
        /// Updates an existing inventory type.
        /// </summary>
        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] WMSInventoryTypesUpdateDTO dto, CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.UpdateAsync(CurrentCompanyId, id, dto, ct),
                ct);
        }

        /// <summary>
        /// Activates or deactivates an inventory type.
        /// </summary>
        [HttpPatch("SetActive/{id:int}")]
        public async Task<IActionResult> SetActive(int id, [FromQuery] bool isActive, CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.SetActiveAsync(CurrentCompanyId, id, isActive, ct),
                ct);
        }
    }
}
