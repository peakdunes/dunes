using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Masters.Cities;
using DUNES.API.ServicesWMS.Masters.Countries;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.Cities
{
    /// <summary>
    /// cities controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CitiesWMSController : BaseController
    {
      
        private readonly ICitiesWMSAPIService _service;

        /// <summary>
        /// constructor dependency injection
        /// </summary>
        /// <param name="service"></param>
        public CitiesWMSController(ICitiesWMSAPIService service)
        {
            _service = service;
        }

        /// <summary>
        /// Return all cities
        /// </summary>
        /// <param name="countryid"></param>
        /// <param name="ct"></param>
        /// 
        /// <returns></returns>
        [HttpGet("all-cities/{countryid}")]
        [ProducesResponseType(typeof(ApiResponse<List<WMSCitiesDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<WMSCitiesDTO>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllCitiesAsync(int countryid, CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetAllAsync(countryid, ct),
                ct);
        }

        /// <summary>
        /// Return all active cities
        /// </summary>
        /// <param name="countryid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("active-cities/{countryid}")]
        [ProducesResponseType(typeof(ApiResponse<List<WMSCitiesDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<WMSCitiesDTO>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetActiveCitiesAsync(int countryid, CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetActiveAsync(countryid, ct),
                ct);
        }

        /// <summary>
        /// Return city information by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("city-by-id/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<WMSCitiesDTO?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<WMSCitiesDTO?>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCityByIdAsync(int id, CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetByIdAsync(id, ct),
                ct);
        }

        /// <summary>
        /// Create a new city
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("create-city")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCityAsync([FromBody] WMSCitiesDTO model, CancellationToken ct)
        {
            // Si tienes [ApiController] y DataAnnotations en el modelo, aquí ya viene validado.
            return await HandleApi(
                ct => _service.CreateAsync(model, ct),
                ct);
        }

        /// <summary>
        /// Update city
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPut("update-city")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCityAsync([FromBody] WMSCitiesDTO model, CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.UpdateAsync(model, ct),
                ct);
        }

        /// <summary>
        /// Activate / Deactivate city
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPut("set-active-city/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SetActiveCityAsync(int id, [FromQuery] bool isActive, CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.SetActiveAsync(id, isActive, ct),
                ct);
        }
    }
}
