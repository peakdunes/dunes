using DUNES.API.Models.Masters;
using DUNES.API.Services.Masters;

using DUNES.Shared.DTOs.Masters;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.Controllers.Masters
{

    /// <summary>
    /// WMS Client companies
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WmsCompanyclientController : BaseController
    {
        private readonly IMasterService<WmsCompanyclient, WmsCompanyclientDto> _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="WmsCompanyclient"/> class.
        /// </summary>
        /// <param name="service">The master service for Consignment Call Types.</param>
        public WmsCompanyclientController(IMasterService<WmsCompanyclient, WmsCompanyclientDto> service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all client companies.
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<WmsCompanyclient>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetAllAsync(ct), ct);
        }

        /// <summary>
        ///  Retrieves a specific client company by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<WmsCompanyclient>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetByIdAsync(id, ct), ct);
        }

        /// <summary>
        /// Creates a new client company.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<WmsCompanyclientDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] WmsCompanyclientDto item, CancellationToken ct)
        {
            return await HandleApi(async ct =>
            {
                var created = await _service.AddAsync(item, ct);
                return created;
            }, ct);
        }

        /// <summary>
        /// Updates an existing client company.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<WmsCompanyclientDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] WmsCompanyclientDto item, CancellationToken ct)
        {
            return await HandleApi(async ct =>
            {
                var updated = await _service.UpdateAsync(item, ct);
                return updated;
            }, ct);
        }

        /// <summary>
        /// Deletes a client company by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            return await HandleApi(ct => _service.DeleteByIdAsync(id, ct), ct);
        }

        /// <summary>
        /// Get all information for a company client by Name
        /// </summary>
        /// <param name="companyname"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<WmsCompanyclientDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("client-company-information-by-name/{companyname}")]
        public async Task<IActionResult> GetByName(string companyname, CancellationToken ct)
        {
            return await HandleApi(ct => _service.SearchByFieldAsync("CompanyId", companyname, ct), ct);
        }
    }
}
