using DUNES.API.Models.Masters;
using DUNES.API.Services.Masters;
using DUNES.API.Utils.Responses;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterInventoryController : BaseController
    {

        private readonly IMasterService<TzebB2bMasterPartDefinition> _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="TzebB2bMasterPartDefinition"/> class.
        /// </summary>
        /// <param name="service">The master service for Inventory Master.</param>
        public MasterInventoryController(IMasterService<TzebB2bMasterPartDefinition> service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all items.
        /// </summary>
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TzebB2bMasterPartDefinition>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return await Handle(() => _service.GetAllAsync());
        }

        /// <summary>
        /// Retrieves a specific item by its ID.
        /// </summary>
        [ProducesResponseType(typeof(ApiResponse<TzebB2bMasterPartDefinition>), 200)]
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
        /// Retrieves a specific item by its ID.
        /// </summary>
        [ProducesResponseType(typeof(ApiResponse<TzebB2bMasterPartDefinition>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetByPartNumber/{partnumber}")]
        public async Task<IActionResult> GetByParNumber(string partnumber)
        {
            return await Handle(async () =>
            {
                var item = await _service.SearchByFieldAsync("PartNo",partnumber);
                if (item == null)
                    return NotFound(ApiResponseFactory.NotFound<object>("Item not found"));
                return Ok(ApiResponseFactory.Ok(item));
            });
        }


        /// <summary>
        /// Creates a new item.
        /// </summary>
        [ProducesResponseType(typeof(ApiResponse<TzebB2bMasterPartDefinition>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TzebB2bMasterPartDefinition item)
        {
            return await Handle(() => _service.AddAsync(item));
        }

        /// <summary>
        /// Updates an existing item.
        /// </summary>
        [ProducesResponseType(typeof(ApiResponse<TzebB2bMasterPartDefinition>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TzebB2bMasterPartDefinition item)
        {
            return await Handle(() => _service.UpdateAsync(item));
        }

        /// <summary>
        /// Deletes a item by its ID.
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
