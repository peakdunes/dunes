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
    /// Controller to manage consignment call type operations (CRUD).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConsignmentCallTypeController : BaseController
    {
        private readonly IMasterService<TzebB2bConsignmentCallsType> _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsignmentCallTypeController"/> class.
        /// </summary>
        /// <param name="service">The master service for Consignment Call Types.</param>
        public ConsignmentCallTypeController(IMasterService<TzebB2bConsignmentCallsType> service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all consignment call types.
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TzebB2bConsignmentCallsType>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetAllAsync( ct), ct);
                     
        }

        /// <summary>
        /// Retrieves a specific consignment call type by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<TzebB2bConsignmentCallsType>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetByIdAsync(id, ct), ct);
                       
        }

        /// <summary>
        /// Creates a new consignment call type.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<TzebB2bConsignmentCallsType>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TzebB2bConsignmentCallsType item, CancellationToken ct)
        {

            return await HandleApi(ct => _service.AddAsync(item, ct), ct);

        }

        /// <summary>
        /// Updates an existing consignment call type.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<TzebB2bConsignmentCallsType>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TzebB2bConsignmentCallsType item, CancellationToken ct)
        {
            return await HandleApi(ct => _service.UpdateAsync(item, ct), ct);

           
        }

        /// <summary>
        /// Deletes a consignment call type by its ID.
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
