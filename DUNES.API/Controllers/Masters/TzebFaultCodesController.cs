using DUNES.API.Models.Masters;
using DUNES.API.Services.Masters;
using DUNES.API.Utils.Responses;
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
        private readonly IMasterService<TzebFaultCodes> _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="TzebFaultCodesController"/> class.
        /// </summary>
        /// <param name="service">The master service for Fault Codes.</param>
        public TzebFaultCodesController(IMasterService<TzebFaultCodes> service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all fault codes.
        /// </summary>
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TzebFaultCodes>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return await Handle(() => _service.GetAllAsync());
        }

        /// <summary>
        /// Retrieves a specific fault code by ID.
        /// </summary>
        /// <param name="id">The ID of the fault code to retrieve.</param>
        [ProducesResponseType(typeof(ApiResponse<TzebFaultCodes>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return await Handle(async () =>
            {
                var item = await _service.GetByIdAsync(id);
                if (item == null)
                    return NotFound(ApiResponseFactory.NotFound<object>("Fault code not found"));
                return Ok(ApiResponseFactory.Ok(item));
            });
        }

        /// <summary>
        /// Creates a new fault code.
        /// </summary>
        /// <param name="item">The fault code to create.</param>
        [ProducesResponseType(typeof(ApiResponse<TzebFaultCodes>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TzebFaultCodes item)
        {
            return await Handle(() => _service.AddAsync(item));
        }

        /// <summary>
        /// Updates an existing fault code.
        /// </summary>
        /// <param name="item">The fault code to update.</param>
        [ProducesResponseType(typeof(ApiResponse<TzebFaultCodes>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TzebFaultCodes item)
        {
            return await Handle(() => _service.UpdateAsync(item));
        }

        /// <summary>
        /// Deletes a fault code by ID.
        /// </summary>
        /// <param name="id">The ID of the fault code to delete.</param>
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
