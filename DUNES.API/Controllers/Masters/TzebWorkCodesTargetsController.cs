using DUNES.API.Models.Masters;
using DUNES.API.Services.Masters;
using DUNES.API.Utils.Responses;
using DUNES.Shared.DTOs.Masters;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.Controllers.Masters
{
    /// <summary>
    /// Controller to manage Work Codes Targets (CRUD operations).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TzebWorkCodesTargetsController : BaseController
    {
        private readonly IMasterService<TzebWorkCodesTargets, TzebWorkCodesTargetsDto> _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="TzebWorkCodesTargetsController"/> class.
        /// </summary>
        /// <param name="service">The master service for Work Codes Targets.</param>
        public TzebWorkCodesTargetsController(IMasterService<TzebWorkCodesTargets, TzebWorkCodesTargetsDto> service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all work codes targets.
        /// </summary>
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TzebWorkCodesTargets>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetAllAsync(ct), ct);
        }

        /// <summary>
        /// Retrieves a specific work code target by ID.
        /// </summary>
        /// <param name="id">The ID of the work code target to retrieve.</param>
        [ProducesResponseType(typeof(ApiResponse<TzebWorkCodesTargets>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetByIdAsync(id, ct), ct);
        }

        /// <summary>
        /// Creates a new work code target.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<TzebWorkCodesTargetsDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TzebWorkCodesTargetsDto item, CancellationToken ct)
        {
            return await HandleApi(async ct =>
            {
                var created = await _service.AddAsync(item, ct);
                return created;
            }, ct);
        }

        /// <summary>
        /// Updates an existing work code target.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<TzebWorkCodesTargetsDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TzebWorkCodesTargetsDto item, CancellationToken ct)
        {
            return await HandleApi(async ct =>
            {
                var updated = await _service.UpdateAsync(item, ct);
                return updated;
            }, ct);
        }

        /// <summary>
        /// Deletes a work code target by ID.
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
