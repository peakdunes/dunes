using DUNES.API.Auth.Authorization;
using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Masters.Locations;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.Locations
{
    /// <summary>
    /// locations controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [RequiresPermission("MODELSWMS.MASTERS.LOCATIONS.ACCESS")]
    public class LocationsWMSController : BaseController
    {
        private readonly ILocationsWMSAPIService _service;

        /// <summary>
        /// constructor (DI)
        /// </summary>
        /// <param name="service"></param>
        public LocationsWMSController(ILocationsWMSAPIService service)
        {
            _service = service;
        }

        /// <summary>
        /// Return all locations
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("wms-all-locations")]
        [ProducesResponseType(typeof(ApiResponse<List<WMSLocationsDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<WMSLocationsDTO>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllLocationsAsync(CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetAllAsync(CurrentCompanyId, ct),
                ct);
        }

        /// <summary>
        /// Return all active locations
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("wms-active-locations")]
        [ProducesResponseType(typeof(ApiResponse<List<WMSLocationsDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<WMSLocationsDTO>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetActiveCountriesAsync(CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetActiveAsync(CurrentCompanyId, ct),
                ct);
        }

        /// <summary>
        /// Return country information by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("wms-location-by-id/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<WMSLocationsDTO?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<WMSLocationsDTO?>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetLocationByIdAsync(int id, CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetByIdAsync(CurrentCompanyId, id, ct),
                ct);
        }

        /// <summary>
        /// Return location information by id
        /// </summary>
        /// <param name="name"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("wms-location-exists-by-name")]
        [ProducesResponseType(typeof(ApiResponse<WMSLocationsDTO?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<WMSLocationsDTO?>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ExistsByNameAsync(
            [FromQuery] string name,
            [FromQuery] int? excludeId,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.ExistsByNameAsync(CurrentCompanyId, name, excludeId, ct),
                ct);
        }

        /// <summary>
        /// Create a new location
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// 
        [RequiresPermission("MODELSWMS.MASTERS.LOCATIONS.CREATE")]
        [HttpPost("wms-create-location")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateLocationAsync(
            [FromBody] WMSLocationsDTO model,
            CancellationToken ct)
        {
            // Si tienes [ApiController] y DataAnnotations en el modelo, aquí ya viene validado.
            return await HandleApi(
                ct => _service.CreateAsync(CurrentCompanyId, model, ct),
                ct);
        }

        /// <summary>
        /// Update country
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// 
        [RequiresPermission("MODELSWMS.MASTERS.LOCATIONS.UPDATE")]
        [HttpPut("wms-update-location")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCountryAsync(
            [FromBody] WMSLocationsDTO model,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.UpdateAsync(CurrentCompanyId, model, ct),
                ct);
        }

        /// <summary>
        /// Activate / Deactivate Location
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// 
        [RequiresPermission("MODELSWMS.MASTERS.LOCATIONS.UPDATE")]
        [HttpPut("wms-set-active-location/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SetActiveCountryAsync(
            int id,
            [FromQuery] bool isActive,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.SetActiveAsync(CurrentCompanyId, id, isActive, ct),
                ct);
        }
    }
}
