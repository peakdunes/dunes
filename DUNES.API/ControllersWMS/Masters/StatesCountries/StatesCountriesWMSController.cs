using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Masters.Countries;
using DUNES.API.ServicesWMS.Masters.StateCountries;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.StatesCountries
{
    /// <summary>
    /// States Countries controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatesCountriesWMSController : BaseController
    {

        private readonly IStateCountriesWMSAPIService _service;

        /// <summary>
        /// constructor dependency injection
        /// </summary>
        /// <param name="service"></param>
        public StatesCountriesWMSController(IStateCountriesWMSAPIService service)
        {
            _service = service;
        }

        /// <summary>
        /// Return all countries
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("all-states-by-countries")]
        [ProducesResponseType(typeof(ApiResponse<List<WMSStatesCountriesDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<WMSStatesCountriesDTO>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllStatesCountriesAsync(CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetAllAsync(ct),
                ct);
        }

        /// <summary>
        /// Return all active countries
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("active-states-countries")]
        [ProducesResponseType(typeof(ApiResponse<List<WMSStatesCountriesDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<WMSStatesCountriesDTO>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetActiveStatesCountriesAsync(CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetActiveAsync(ct),
                ct);
        }

        /// <summary>
        /// Return country information by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("state-country-by-id/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<WMSStatesCountriesDTO?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<WMSStatesCountriesDTO?>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetStateCountryByIdAsync(int id, CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetByIdAsync(id, ct),
                ct);
        }

        /// <summary>
        /// Create a new country
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost("create-state-country")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateStateCountryAsync([FromBody] WMSStatesCountriesDTO model, CancellationToken ct)
        {
            // Si tienes [ApiController] y DataAnnotations en el modelo, aquí ya viene validado.
            return await HandleApi(
                ct => _service.CreateAsync(model, ct),
                ct);
        }

        /// <summary>
        /// Update country
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPut("update-state-country")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStateCountryAsync([FromBody] WMSStatesCountriesDTO model, CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.UpdateAsync(model, ct),
                ct);
        }

        /// <summary>
        /// Activate / Deactivate country
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPut("set-active-state-country/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SetActiveStateCountryAsync(int id, [FromQuery] bool isActive, CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.SetActiveAsync(id, isActive, ct),
                ct);
        }

    }
}
