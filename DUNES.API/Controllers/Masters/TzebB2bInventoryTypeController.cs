using DUNES.API.Models.Masters;
using DUNES.API.Services.Masters;
using DUNES.API.Utils.Responses;
using DUNES.Shared.DTOs.Masters;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.Controllers.Masters
{
    /// <summary>
    /// Controller to manage Inventory Types for B2B operations (CRUD).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TzebB2bInventoryTypeController : BaseController
    {
        private readonly IMasterService<TzebB2bInventoryType, TzebB2bInventoryTypeDto> _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="TzebB2bInventoryTypeController"/> class.
        /// </summary>
        /// <param name="service">The master service for Inventory Types.</param>
        public TzebB2bInventoryTypeController(IMasterService<TzebB2bInventoryType, TzebB2bInventoryTypeDto> service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all inventory types.
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TzebB2bInventoryType>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetAllAsync(ct), ct);
        }

        /// <summary>
        /// Retrieves a specific inventory type by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<TzebB2bInventoryType>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetByIdAsync(id, ct), ct);
        }


        /// <summary>
        /// Creates a new inventory type.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<TzebB2bInventoryTypeDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TzebB2bInventoryTypeDto item, CancellationToken ct)
        {
            return await HandleApi(async ct =>
            {
                var created = await _service.AddAsync(item, ct);
                return created;
            }, ct);
        }

        /// <summary>
        /// Updates an existing inventory type.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<TzebB2bInventoryTypeDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TzebB2bInventoryTypeDto item, CancellationToken ct)
        {
            return await HandleApi(async ct =>
            {
                var updated = await _service.UpdateAsync(item, ct);
                return updated;
            }, ct);
        }

        /// <summary>
        /// Deletes an inventory type by ID.
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