using DUNES.API.ServicesWMS.Masters.Bins;
using DUNES.Shared.DTOs.WMS;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.Bins
{
    /// <summary>
    /// Bins WMS Controller
    /// Scoped by Company + Location + Rack
    /// </summary>
    [ApiController]
    [Route("api/wms/companies/{companyId:int}/locations/{locationId:int}/racks/{rackId:int}/bins")]
    public class BinsWMSController : ControllerBase
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
            int companyId,
            int locationId,
            int rackId,
            CancellationToken ct)
        {
            var result = await _service.GetAllAsync(companyId, locationId, rackId, ct);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get all active bins
        /// </summary>
        [HttpGet("GetActive")]
        public async Task<IActionResult> GetActive(
            int companyId,
            int locationId,
            int rackId,
            CancellationToken ct)
        {
            var result = await _service.GetActiveAsync(companyId, locationId, rackId, ct);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Get bin by id
        /// </summary>
        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(
            int companyId,
            int locationId,
            int rackId,
            int id,
            CancellationToken ct)
        {
            var result = await _service.GetByIdAsync(companyId, locationId, rackId, id, ct);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Create new bin
        /// </summary>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            int companyId,
            int locationId,
            int rackId,
            [FromBody] WMSBinsDto dto,
            CancellationToken ct)
        {
            dto.Idcompany = companyId;
            dto.LocationsId = locationId;
            dto.RacksId = rackId;

            var result = await _service.CreateAsync(dto, ct);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Update bin
        /// </summary>
        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(
            int companyId,
            int locationId,
            int rackId,
            int id,
            [FromBody] WMSBinsDto dto,
            CancellationToken ct)
        {
            dto.Idcompany = companyId;
            dto.LocationsId = locationId;
            dto.RacksId = rackId;
            dto.Id = id;

            var result = await _service.UpdateAsync(dto, ct);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Activate / Deactivate bin
        /// </summary>
        [HttpPatch("SetActive/{id:int}")]
        public async Task<IActionResult> SetActive(
            int companyId,
            int locationId,
            int rackId,
            int id,
            [FromQuery] bool isActive,
            CancellationToken ct)
        {
            var result = await _service.SetActiveAsync(companyId, locationId, rackId, id, isActive, ct);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Check if bin name exists
        /// </summary>
        [HttpGet("ExistsByName")]
        public async Task<IActionResult> ExistsByName(
            int companyId,
            int locationId,
            int rackId,
            [FromQuery] string name,
            [FromQuery] int? excludeId,
            CancellationToken ct)
        {
            var result = await _service.ExistsByNameAsync(
                companyId,
                locationId,
                rackId,
                name,
                excludeId,
                ct);

            return StatusCode(result.StatusCode, result);
        }
    }
}
