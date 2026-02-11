using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Masters.CompanyClientInventoryCategory;
using DUNES.Shared.DTOs.WMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.CompanyClientInventoryCategory
{
    /// <summary>
    /// API controller for managing inventory categories assigned to a client.
    /// 
    /// All actions are scoped by:
    /// - CompanyId (from token)
    /// - CompanyClientId (from token)
    /// 
    /// This controller contains NO business logic.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/wms/masters/company-client/inventory-categories")]
    public class CompanyClientInventoryCategoryWMSController : BaseController
    {
        private readonly ICompanyClientInventoryCategoryService _service;

        /// <summary>
        /// Constructor (DI)
        /// </summary>
        public CompanyClientInventoryCategoryWMSController(
            ICompanyClientInventoryCategoryService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all inventory categories assigned to this client.
        /// </summary>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetAllAsync(CurrentCompanyId, CurrentCompanyClientId, ct),
                ct);
        }

        /// <summary>
        /// Get a specific client-category mapping by Id.
        /// </summary>
        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetByIdAsync(CurrentCompanyId, CurrentCompanyClientId, id, ct),
                ct);
        }

        /// <summary>
        /// Create a new client-category mapping.
        /// </summary>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            [FromBody] WMSCompanyClientInventoryCategoryCreateDTO dto,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.CreateAsync(CurrentCompanyId, CurrentCompanyClientId, dto, ct),
                ct);
        }

        /// <summary>
        /// Update an existing client-category mapping.
        /// </summary>
        [HttpPut("Update")]
        public async Task<IActionResult> Update(
            [FromBody] WMSCompanyClientInventoryCategoryUpdateDTO dto,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.UpdateAsync(CurrentCompanyId, CurrentCompanyClientId, dto, ct),
                ct);
        }

        /// <summary>
        /// Activate or deactivate a client-category mapping.
        /// </summary>
        [HttpPatch("SetActive/{id:int}")]
        public async Task<IActionResult> SetActive(
            int id,
            [FromQuery] bool isActive,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.SetActiveAsync(CurrentCompanyId, CurrentCompanyClientId, id, isActive, ct),
                ct);
        }
    }
}
