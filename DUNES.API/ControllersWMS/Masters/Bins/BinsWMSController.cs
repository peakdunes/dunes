using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Masters.Bins;
using DUNES.Shared.DTOs.WMS;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.Bins
{
    /// <summary>
    /// Bins WMS Controller
    /// Scoped by Company (from token) + Location + Rack
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BinsWMSController : BaseController
    {
        private readonly IBinsWMSAPIService _service;

        /// <summary>
        /// Constructor (DI)
        /// </summary>
        public BinsWMSController(IBinsWMSAPIService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all bins
        /// </summary>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(
            int locationId,
            int rackId,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetAllAsync(
                    CurrentCompanyId,
                    locationId,
                    rackId,
                    ct),
                ct);
        }

        /// <summary>
        /// Get all active bins
        /// </summary>
        [HttpGet("GetActive")]
        public async Task<IActionResult> GetActive(
            int locationId,
            int rackId,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetActiveAsync(
                    CurrentCompanyId,
                    locationId,
                    rackId,
                    ct),
                ct);
        }

        /// <summary>
        /// Get bin by id
        /// </summary>
        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(
            int locationId,
            int rackId,
            int id,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetByIdAsync(
                    CurrentCompanyId,
                    locationId,
                    rackId,
                    id,
                    ct),
                ct);
        }

        /// <summary>
        /// Create new bin
        /// </summary>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            int locationId,
            int rackId,
            [FromBody] WMSBinsCreateDto dto,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.CreateAsync(
                    CurrentCompanyId,
                    locationId,
                    rackId,
                    dto,
                    ct),
                ct);
        }

        /// <summary>
        /// Update bin
        /// </summary>
        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(
            int locationId,
            int rackId,
            int id,
            [FromBody] WMSBinsCreateDto dto,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.UpdateAsync(
                    CurrentCompanyId,
                    locationId,
                    rackId,
                    id,
                    dto,
                    ct),
                ct);
        }

        /// <summary>
        /// Activate / Deactivate bin
        /// </summary>
        [HttpPatch("SetActive/{id:int}")]
        public async Task<IActionResult> SetActive(
            int locationId,
            int rackId,
            int id,
            [FromQuery] bool isActive,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.SetActiveAsync(
                    CurrentCompanyId,
                    locationId,
                    rackId,
                    id,
                    isActive,
                    ct),
                ct);
        }

        /// <summary>
        /// Check if bin name exists
        /// </summary>
        [HttpGet("ExistsByName")]
        public async Task<IActionResult> ExistsByName(
            int locationId,
            int rackId,
            [FromQuery] string name,
            [FromQuery] int? excludeId,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.ExistsByNameAsync(
                    CurrentCompanyId,
                    locationId,
                    rackId,
                    name,
                    excludeId,
                    ct),
                ct);
        }
    }
}
