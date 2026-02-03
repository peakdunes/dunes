using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Masters.Racks;
using DUNES.Shared.DTOs.WMS;
using Humanizer;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.Racks
{
    /// <summary>
    /// Racks Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RacksWMSController : BaseController
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
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(
            int locationId,
            CancellationToken ct)
        {
            
            return await Handle(
               ct => _service.GetAllAsync(CurrentCompanyId, locationId, ct),
               ct);

        }

        /// <summary>
        /// Get all active racks by company and location
        /// </summary>
        [HttpGet("GetActive")]
        public async Task<IActionResult> GetActive(
            int locationId,
            CancellationToken ct)
        {
            

            return await Handle(
                ct => _service.GetActiveAsync(CurrentCompanyId, locationId, ct),
                ct);


        }

        /// <summary>
        /// Get rack by id
        /// </summary>
        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(
            int id,
            int locationId,
            CancellationToken ct)
        {
            
            return await Handle(
               ct => _service.GetByIdAsync(CurrentCompanyId, locationId, id, ct),
               ct);


        }

        /// <summary>
        /// Create new rack
        /// </summary>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            int locationId,
            [FromBody] WMSRacksCreateDTO dto,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.CreateAsync(
                    CurrentCompanyId,
                    locationId,
                    dto,
                    ct),
                ct);
        }

        /// <summary>
        /// Update rack
        /// </summary>
        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            int locationId,
            [FromBody] WMSRacksCreateDTO dto,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.UpdateAsync(
                    CurrentCompanyId,
                    locationId,
                    id,
                    dto,
                    ct),
                ct);
        }

        /// <summary>
        /// Activate / Deactivate rack
        /// </summary>
        [HttpPatch("SetActive/{id:int}")]
        public async Task<IActionResult> SetActive(
            int id,
            int locationId,
            [FromQuery] bool isActive,
            CancellationToken ct)
        {
           

            return await Handle(
          ct => _service.SetActiveAsync(CurrentCompanyId, locationId, id, isActive, ct),
          ct);
        }

        /// <summary>
        /// Check if rack name exists for company and location
        /// </summary>
        [HttpGet("ExistsByName")]
        public async Task<IActionResult> ExistsByName(
            int locationId,
            [FromQuery] string name,
            [FromQuery] int? excludeId,
            CancellationToken ct)
        {
           

            return await Handle(ct => _service.ExistsByNameAsync(CurrentCompanyId,locationId,name,excludeId,ct),ct);

        }
    }
}
