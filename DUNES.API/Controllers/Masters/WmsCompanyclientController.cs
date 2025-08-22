using DUNES.API.Models.Masters;
using DUNES.API.Services.Masters;
using DUNES.API.Utils.Responses;
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
        private readonly IMasterService<WmsCompanyclient> _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="WmsCompanyclient"/> class.
        /// </summary>
        /// <param name="service">The master service for Consignment Call Types.</param>
        public WmsCompanyclientController(IMasterService<WmsCompanyclient> service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all client companies.
        /// </summary>
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<WmsCompanyclient>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return await Handle(() => _service.GetAllAsync());
        }

        /// <summary>
        /// Retrieves a specific client company by ID.
        /// </summary>
        [ProducesResponseType(typeof(ApiResponse<WmsCompanyclient>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return await Handle(async () =>
            {
                var item = await _service.GetByIdAsync(id);
                if (item == null)
                    return NotFound(ApiResponseFactory.NotFound<object>("Item not found"));
                return Ok(ApiResponseFactory.Ok(item));
            });
        }

        /// <summary>
        /// Creates a new client company.
        /// </summary>
        [ProducesResponseType(typeof(ApiResponse<WmsCompanyclient>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] WmsCompanyclient item)
        {
            return await Handle(() => _service.AddAsync(item));
        }

        /// <summary>
        /// Updates an existing client company.
        /// </summary>
        [ProducesResponseType(typeof(ApiResponse<WmsCompanyclient>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] WmsCompanyclient item)
        {
            return await Handle(() => _service.UpdateAsync(item));
        }

        /// <summary>
        /// Deletes a client company by its ID.
        /// </summary>
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
