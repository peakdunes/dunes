using DUNES.API.Models.Masters;
using DUNES.API.Services.Masters;
using DUNES.API.Utils.Responses;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.Controllers.Masters
{
    /// <summary>
    /// Controller to manage Repair Actions Codes (CRUD operations).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TrepairActionsCodesController : BaseController
    {
        private readonly IMasterService<TrepairActionsCodes> _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrepairActionsCodesController"/> class.
        /// </summary>
        /// <param name="service">The master service for Repair Actions Codes.</param>
        public TrepairActionsCodesController(IMasterService<TrepairActionsCodes> service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all Repair Actions Codes.
        /// </summary>
        /// <returns>A list of all available action codes.</returns>
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TrepairActionsCodes>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return await Handle(() => _service.GetAllAsync());
        }

        /// <summary>
        /// Retrieves a specific Repair Action Code by ID.
        /// </summary>
        /// <param name="id">The ID of the action code to retrieve.</param>
        /// <returns>The matching action code, if found.</returns>
        [ProducesResponseType(typeof(ApiResponse<TrepairActionsCodes>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return await Handle(async () =>
            {
                var item = await _service.GetByIdAsync(id);
                if (item == null)
                    return NotFound(ApiResponseFactory.NotFound<object>("Action code not found"));
                return Ok(ApiResponseFactory.Ok(item));
            });
        }

        /// <summary>
        /// Creates a new Repair Action Code.
        /// </summary>
        /// <param name="item">The new action code to create.</param>
        /// <returns>The created action code.</returns>
        [ProducesResponseType(typeof(ApiResponse<TrepairActionsCodes>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TrepairActionsCodes item)
        {
            return await Handle(() => _service.AddAsync(item));
        }

        /// <summary>
        /// Updates an existing Repair Action Code.
        /// </summary>
        /// <param name="item">The action code to update.</param>
        /// <returns>The updated action code.</returns>
        [ProducesResponseType(typeof(ApiResponse<TrepairActionsCodes>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TrepairActionsCodes item)
        {
            return await Handle(() => _service.UpdateAsync(item));
        }

        /// <summary>
        /// Deletes a Repair Action Code by ID.
        /// </summary>
        /// <param name="id">The ID of the action code to delete.</param>
        /// <returns>True if the code was successfully deleted.</returns>
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
