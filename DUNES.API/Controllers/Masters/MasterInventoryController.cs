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
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TzebB2bMasterPartDefinition>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetAllAsync(ct), ct);
        }

        /// <summary>
        /// Retrieves a specific item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<TzebB2bMasterPartDefinition>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetByIdAsync(id, ct), ct);
        }


        /// <summary>
        /// Retrieves a specific item by its ID.
        /// </summary>
        /// <param name="partnumber"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<TzebB2bMasterPartDefinition>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetByPartNumber/{partnumber}")]
        public async Task<IActionResult> GetByParNumber(string partnumber, CancellationToken ct)
        {
            return await HandleApi(ct => _service.SearchByFieldAsync("PartNo", partnumber, ct), ct);
        }


        /// <summary>
        /// Creates a new item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<TzebB2bMasterPartDefinition>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TzebB2bMasterPartDefinition item, CancellationToken ct)
        {
            return await HandleApi(async ct =>
            {
                var created = await _service.AddAsync(item, ct);
                return created;
            }, ct);
        }

        /// <summary>
        /// Updates an existing item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<TzebB2bMasterPartDefinition>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TzebB2bMasterPartDefinition item, CancellationToken ct)
        {
            return await HandleApi(async ct =>
            {
                var updated = await _service.UpdateAsync(item, ct);
                return updated;
            }, ct);
        }

        /// <summary>
        /// Deletes a item by its ID.
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
