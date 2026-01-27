using DUNES.API.ServicesWMS.Masters.Racks;
using DUNES.Shared.DTOs.WMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.Racks
{

    /// <summary>
    /// Racks Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RacksWMSController : ControllerBase
    {

        private readonly IRacksWMSAPIService _service;

        /// <summary>
        /// Constructor (DI)
        /// </summary>
        public RacksWMSController(IRacksWMSAPIService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all racks by company and location
        /// </summary>
        [HttpGet("wms-all-racks")]
        public async Task<IActionResult> GetAll(
            int companyId,
            int locationId,
            CancellationToken ct)
        {
            var result = await _service.GetAllAsync(companyId, locationId, ct);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get all active racks by company and location
        /// </summary>
        [HttpGet("wms-active_racks")]
        public async Task<IActionResult> GetActive(
            int companyId,
            int locationId,
            CancellationToken ct)
        {
            var result = await _service.GetActiveAsync(companyId, locationId, ct);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get rack by id
        /// </summary>
        [HttpGet("wms-rack-by-id/{id:int}")]
        public async Task<IActionResult> GetById(
            int companyId,
            int locationId,
            int id,
            CancellationToken ct)
        {
            var result = await _service.GetByIdAsync(companyId, locationId, id, ct);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Create new rack
        /// </summary>
        [HttpPost("wms-create-rack")]
        public async Task<IActionResult> Create(
            int companyId,
            int locationId,
            [FromBody] WMSRacksDTO dto,
            CancellationToken ct)
        {
            dto.Idcompany = companyId;
            dto.LocationsId = locationId;

            var result = await _service.CreateAsync(dto, ct);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Update rack
        /// </summary>
        [HttpPut("wms-update-rack/{id:int}")]
        public async Task<IActionResult> Update(
            int companyId,
            int locationId,
            int id,
            [FromBody] WMSRacksDTO dto,
            CancellationToken ct)
        {
            dto.Idcompany = companyId;
            dto.LocationsId = locationId;
            dto.Id = id;

            var result = await _service.UpdateAsync(dto, ct);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Activate / Deactivate rack
        /// </summary>
        [HttpPatch("wms-set-active-rack/{id:int}")]
        public async Task<IActionResult> SetActive(
            int companyId,
            int locationId,
            int id,
            [FromQuery] bool isActive,
            CancellationToken ct)
        {
            var result = await _service.SetActiveAsync(companyId, locationId, id, isActive, ct);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Check if rack name exists for company and location
        /// </summary>
        [HttpGet("wms-rack-exists-by-name")]
        public async Task<IActionResult> ExistsByName(
            int companyId,
            int locationId,
            [FromQuery] string name,
            [FromQuery] int? excludeId,
            CancellationToken ct)
        {
            var result = await _service.ExistsByNameAsync(companyId, locationId, name, excludeId, ct);
            return StatusCode(result.StatusCode, result);
        }

    }
}
