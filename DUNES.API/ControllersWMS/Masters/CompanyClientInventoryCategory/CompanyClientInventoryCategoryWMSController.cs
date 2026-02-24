using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Masters.CompanyClientInventoryCategory;
using DUNES.Shared.DTOs.WMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.CompanyClientInventoryCategory
{
    /// <summary>
    /// API controller for managing inventory categories enabled for the current client.
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
        private readonly ICompanyClientInventoryCategoryWMSAPIService _service;

        /// <summary>Constructor (DI).</summary>
        public CompanyClientInventoryCategoryWMSController(ICompanyClientInventoryCategoryWMSAPIService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all inventory categories for this client.
        /// Returns only:
        /// - mapping IsActive=true AND
        /// - master catalog IsActive=true
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetAllAsync(CurrentCompanyId, CurrentCompanyClientId, ct),
                ct);
        }


        /// <summary>
        /// Get enabled inventory categories for this client.
        /// Returns only:
        /// - mapping IsActive=true AND
        /// - master catalog IsActive=true
        /// </summary>
        [HttpGet("GetEnabled")]
        public async Task<IActionResult> GetEnabled(CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetEnabledAsync(CurrentCompanyId, CurrentCompanyClientId, ct),
                ct);
        }

      

        /// <summary>
        /// Get a specific client-category mapping by Id (scoped).
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
        /// Note: Master catalog must be active to allow creation/enabling.
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
        /// Activate or deactivate a client-category mapping by mapping Id.
        /// Note: Activation must be rejected if master category is inactive.
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

        /// <summary>
        /// Replace the enabled set for the current client (bulk, anti-error).
        /// Body: list of master InventoryCategoryIds that should be enabled.
        /// </summary>
        [HttpPut("SetEnabledSet")]
        public async Task<IActionResult> SetEnabledSet(
            [FromBody] List<int> inventoryCategoryIds,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.SetEnabledSetAsync(CurrentCompanyId, CurrentCompanyClientId, inventoryCategoryIds, ct),
                ct);
        }
    }
}
