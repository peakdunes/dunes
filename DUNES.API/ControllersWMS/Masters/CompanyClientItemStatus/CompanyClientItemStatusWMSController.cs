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
    /// API controller for managing item statuses enabled for the current client.
    ///
    /// Scoped by:
    /// - CompanyId (from token)
    /// - CompanyClientId (from token)
    ///
    /// This controller contains NO business logic.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/wms/masters/company-client/item-statuses")]
    public class CompanyClientItemStatusWMSController : BaseController
    {
        private readonly ICompanyClientItemStatusService _service;

        /// <summary>Constructor (DI).</summary>
        public CompanyClientItemStatusWMSController(ICompanyClientItemStatusService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get enabled item statuses for this client.
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
        /// Backwards compatibility: previous route "GetAll" returns enabled results.
        /// </summary>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetEnabledAsync(CurrentCompanyId, CurrentCompanyClientId, ct),
                ct);
        }

        /// <summary>
        /// Get a specific mapping by Id (scoped).
        /// </summary>
        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetByIdAsync(CurrentCompanyId, CurrentCompanyClientId, id, ct),
                ct);
        }

        /// <summary>
        /// Create a new mapping between the current client and a master item status.
        /// Note: master catalog must be active to allow creation/enabling.
        /// </summary>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            [FromBody] WMSCompanyClientItemStatusCreateDTO dto,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.CreateAsync(CurrentCompanyId, CurrentCompanyClientId, dto, ct),
                ct);
        }

        /// <summary>
        /// Activate or deactivate a mapping by mapping Id.
        /// Note: activation must be rejected if master item status is inactive.
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
        /// Body: list of master ItemStatusIds that should be enabled.
        /// </summary>
        [HttpPut("SetEnabledSet")]
        public async Task<IActionResult> SetEnabledSet(
            [FromBody] List<int> itemStatusIds,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.SetEnabledSetAsync(CurrentCompanyId, CurrentCompanyClientId, itemStatusIds, ct),
                ct);
        }
    }
}
