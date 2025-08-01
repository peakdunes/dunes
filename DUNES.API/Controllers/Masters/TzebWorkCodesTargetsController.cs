using DUNES.API.Models.Masters;
using DUNES.API.Services.Masters;
using DUNES.API.Utils.Responses;
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
        private readonly IMasterService<TzebWorkCodesTargets> _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="TzebWorkCodesTargetsController"/> class.
        /// </summary>
        /// <param name="service">The master service for Work Codes Targets.</param>
        public TzebWorkCodesTargetsController(IMasterService<TzebWorkCodesTargets> service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all work codes targets.
        /// </summary>
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TzebWorkCodesTargets>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return await Handle(() => _service.GetAllAsync());
        }

        /// <summary>
        /// Retrieves a specific work code target by ID.
        /// </summary>
        /// <param name="id">The ID of the work code target to retrieve.</param>
        [ProducesResponseType(typeof(ApiResponse<TzebWorkCodesTargets>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return await Handle(async () =>
            {
                var item = await _service.GetByIdAsync(id);
                if (item == null)
                    return NotFound(ApiResponseFactory.NotFound<object>("Work code target not found"));
                return Ok(ApiResponseFactory.Ok(item));
            });
        }

        /// <summary>
        /// Creates a new work code target.
        /// </summary>
        /// <param name="item">The work code target to create.</param>
        [ProducesResponseType(typeof(ApiResponse<TzebWorkCodesTargets>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TzebWorkCodesTargets item)
        {
            return await Handle(() => _service.AddAsync(item));
        }

        /// <summary>
        /// Updates an existing work code target.
        /// </summary>
        /// <param name="item">The work code target to update.</param>
        [ProducesResponseType(typeof(ApiResponse<TzebWorkCodesTargets>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TzebWorkCodesTargets item)
        {
            return await Handle(() => _service.UpdateAsync(item));
        }

        /// <summary>
        /// Deletes a work code target by ID.
        /// </summary>
        /// <param name="id">The ID of the work code target to delete.</param>
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await Handle(() => _service.DeleteByIdAsync(id));
        }
    }

}
