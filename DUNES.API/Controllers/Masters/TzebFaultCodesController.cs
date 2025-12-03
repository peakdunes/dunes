using DUNES.API.Models.Masters;
using DUNES.API.Services.Masters;

using DUNES.Shared.DTOs.Masters;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.Controllers.Masters
{
    /// <summary>
    /// Controller to manage Fault Codes (CRUD operations).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TzebFaultCodesController : BaseController
    {
        private readonly IMasterService<TzebFaultCodes, TzebFaultCodesDto> _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="TzebFaultCodesController"/> class.
        /// </summary>
        /// <param name="service">The master service for Fault Codes.</param>
        public TzebFaultCodesController(IMasterService<TzebFaultCodes, TzebFaultCodesDto> service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all fault codes.
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TzebFaultCodes>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetAllAsync(ct), ct);
        }

        /// <summary>
        /// Retrieves a specific fault code by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<TzebFaultCodes>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetByIdAsync(id, ct), ct);
        }

        /// <summary>
        /// Creates a new fault code.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<TzebFaultCodesDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TzebFaultCodesDto item, CancellationToken ct)
        {
            return await HandleApi(async ct =>
            {
                var created = await _service.AddAsync(item, ct);
                return created;
            }, ct);
        }

        /// <summary>
        /// Updates an existing fault code.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<TzebFaultCodesDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TzebFaultCodesDto item, CancellationToken ct)
        {
            return await HandleApi(async ct =>
            {
                var updated = await _service.UpdateAsync(item, ct);
                return updated;
            }, ct);
        }

        /// <summary>
        /// Deletes a fault code by ID.
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
    }

}
