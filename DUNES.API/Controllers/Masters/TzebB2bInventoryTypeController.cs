using DUNES.API.Models.Masters;
using DUNES.API.Services.Masters;
using DUNES.API.Utils.Responses;
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
        private readonly IMasterService<TzebB2bInventoryType> _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="TzebB2bInventoryTypeController"/> class.
        /// </summary>
        /// <param name="service">The master service for Inventory Types.</param>
        public TzebB2bInventoryTypeController(IMasterService<TzebB2bInventoryType> service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all inventory types.
        /// </summary>
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TzebB2bInventoryType>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return await Handle(() => _service.GetAllAsync());
        }

        /// <summary>
        /// Retrieves a specific inventory type by ID.
        /// </summary>
        /// <param name="id">The ID of the inventory type to retrieve.</param>
        [ProducesResponseType(typeof(ApiResponse<TzebB2bInventoryType>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return await Handle(async () =>
            {
                var item = await _service.GetByIdAsync(id);
                if (item == null)
                    return NotFound(ApiResponseFactory.NotFound<object>("Inventory type not found"));
                return Ok(ApiResponseFactory.Ok(item));
            });
        }

        /// <summary>
        /// Creates a new inventory type.
        /// </summary>
        /// <param name="item">The inventory type to create.</param>
        [ProducesResponseType(typeof(ApiResponse<TzebB2bInventoryType>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TzebB2bInventoryType item)
        {
            return await Handle(() => _service.AddAsync(item));
        }

        /// <summary>
        /// Updates an existing inventory type.
        /// </summary>
        /// <param name="item">The inventory type to update.</param>
        [ProducesResponseType(typeof(ApiResponse<TzebB2bInventoryType>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TzebB2bInventoryType item)
        {
            return await Handle(() => _service.UpdateAsync(item));
        }

        /// <summary>
        /// Deletes an inventory type by ID.
        /// </summary>
        /// <param name="id">The ID of the inventory type to delete.</param>
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