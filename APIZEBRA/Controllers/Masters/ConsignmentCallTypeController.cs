using APIZEBRA.Models.Masters;
using APIZEBRA.Services.Masters;
using APIZEBRA.Utils.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIZEBRA.Controllers.Masters
{

    /// <summary>
    /// Controller to manage consignment call type operations (CRUD).
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    
    public class ConsignmentCallTypeController : ControllerBase
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
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TzebB2bConsignmentCallsType>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetAll")]
        public async Task<ApiResponse<IEnumerable<TzebB2bConsignmentCallsType>>> GetAll()
        {
            return await _service.GetAllAsync();
        }

        /// <summary>
        /// Retrieves a specific consignment call type by its ID.
        /// </summary>
        [ProducesResponseType(typeof(ApiResponse<TzebB2bConsignmentCallsType>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpGet("GetById/{id}")]
        public async Task<ApiResponse<TzebB2bConsignmentCallsType>> GetById(int id)
        {
            return await _service.GetByIdAsync(id);
        }

        /// <summary>
        /// Creates a new consignment call type.
        /// </summary>
        [ProducesResponseType(typeof(ApiResponse<TzebB2bConsignmentCallsType>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPost("Create")]
        public async Task<ApiResponse<TzebB2bConsignmentCallsType>> Create([FromBody] TzebB2bConsignmentCallsType item)
        {
            return await _service.AddAsync(item);
        }

        /// <summary>
        /// Updates an existing consignment call type.
        /// </summary>
        [ProducesResponseType(typeof(ApiResponse<TzebB2bConsignmentCallsType>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpPut("Update")]
        public async Task<ApiResponse<TzebB2bConsignmentCallsType>> Update([FromBody] TzebB2bConsignmentCallsType item)
        {
            return await _service.UpdateAsync(item);
        }

        /// <summary>
        /// Deletes a consignment call type by its ID.
        /// </summary>
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [HttpDelete("Delete/{id}")]
        public async Task<ApiResponse<bool>> Delete(int id)
        {
            return await _service.DeleteByIdAsync(id);
        }

    }
    }
