using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Masters.ItemStatus;
using DUNES.Shared.DTOs.WMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.ItemStatus
{
    /// <summary>
    /// Item Status API Controller (WMS / Masters).
    ///
    /// Provides CRUD and state management endpoints for item statuses.
    ///
    /// IMPORTANT (STANDARD COMPANYID):
    /// - CompanyId is NEVER accepted from route, query, or body.
    /// - CompanyId is always obtained from the authenticated token via BaseController (CurrentCompanyId).
    /// - This controller contains NO business logic; it only delegates to the Service layer.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/wms/masters/item-status")]
    public class ItemStatusWMSController : BaseController
    {
        private readonly IItemStatusWMSAPIService _service;

        /// <summary>
        /// Initializes a new instance of <see cref="ItemStatusWMSController"/>.
        /// </summary>
        /// <param name="service">Item status service.</param>
        public ItemStatusWMSController(IItemStatusWMSAPIService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all item statuses for the current company (tenant).
        /// </summary>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetAllAsync(CurrentCompanyId, ct),
                ct);
        }

        /// <summary>
        /// Retrieves active item statuses for the current company (tenant).
        /// </summary>
        [HttpGet("GetActive")]
        public async Task<IActionResult> GetActive(CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetActiveAsync(CurrentCompanyId, ct),
                ct);
        }

        /// <summary>
        /// Retrieves an item status by its identifier.
        /// </summary>
        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetByIdAsync(CurrentCompanyId, id, ct),
                ct);
        }

        /// <summary>
        /// Creates a new item status.
        /// </summary>
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] WMSItemStatusCreateDTO dto, CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.CreateAsync(CurrentCompanyId, dto, ct),
                ct);
        }

        /// <summary>
        /// Updates an existing item status.
        /// </summary>
        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] WMSItemStatusUpdateDTO dto, CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.UpdateAsync(CurrentCompanyId, id, dto, ct),
                ct);
        }

        /// <summary>
        /// Activates or deactivates an item status.
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
