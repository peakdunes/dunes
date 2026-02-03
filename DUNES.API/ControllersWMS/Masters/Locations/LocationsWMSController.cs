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
    /// Locations WMS Controller
    /// Scoped by Company (CompanyId from token)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [RequiresPermission("MODELSWMS.MASTERS.LOCATIONS.ACCESS")]
    public class LocationsWMSController : BaseController
    {
        private readonly ILocationsWMSAPIService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationsWMSController"/> class.
        /// </summary>
        /// <param name="service">Locations service</param>
        public LocationsWMSController(ILocationsWMSAPIService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets all locations for the current company.
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of locations</returns>
        [HttpGet("wms-all-locations")]
        [ProducesResponseType(typeof(ApiResponse<List<WMSLocationsUpdateDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllLocationsAsync(CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetAllAsync(CurrentCompanyId, ct),
                ct);
        }

        /// <summary>
        /// Gets all active locations for the current company.
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of active locations</returns>
        [HttpGet("wms-active-locations")]
        [ProducesResponseType(typeof(ApiResponse<List<WMSLocationsUpdateDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetActiveLocationsAsync(CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetActiveAsync(CurrentCompanyId, ct),
                ct);
        }

        /// <summary>
        /// Gets location information by identifier.
        /// </summary>
        /// <param name="id">Location identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Location details</returns>
        [HttpGet("wms-location-by-id/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<WMSLocationsUpdateDTO?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetLocationByIdAsync(
            int id,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetByIdAsync(CurrentCompanyId, id, ct),
                ct);
        }

        /// <summary>
        /// Checks if a location with the same name already exists.
        /// </summary>
        /// <param name="name">Location name</param>
        /// <param name="excludeId">Optional location id to exclude from validation</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if exists, otherwise false</returns>
        [HttpGet("wms-location-exists-by-name")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
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
        /// Creates a new location.
        /// </summary>
        /// <param name="model">Location data</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Creation result</returns>
        [RequiresPermission("MODELSWMS.MASTERS.LOCATIONS.CREATE")]
        [HttpPost("wms-create-location")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateLocationAsync(
            [FromBody] WMSLocationsUpdateDTO model,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.CreateAsync(CurrentCompanyId, model, ct),
                ct);
        }

        /// <summary>
        /// Updates an existing location.
        /// </summary>
        /// <param name="model">Updated location data</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Update result</returns>
        [RequiresPermission("MODELSWMS.MASTERS.LOCATIONS.UPDATE")]
        [HttpPut("wms-update-location")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateLocationAsync(
            [FromBody] WMSLocationsUpdateDTO model,
            CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.UpdateAsync(CurrentCompanyId,model.Id, model, ct),
                ct);
        }

        /// <summary>
        /// Activates or deactivates a location.
        /// </summary>
        /// <param name="id">Location identifier</param>
        /// <param name="isActive">Activation state</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Operation result</returns>
        [RequiresPermission("MODELSWMS.MASTERS.LOCATIONS.UPDATE")]
        [HttpPut("wms-set-active-location/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SetActiveLocationAsync(
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
