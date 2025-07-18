using APIZEBRA.Models.Masters;
using APIZEBRA.Services.Masters;
using APIZEBRA.Utils.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIZEBRA.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TzebB2bInventoryTypeController : BaseController
    {

        private readonly IMasterService<TzebB2bInventoryType> _service;

        public TzebB2bInventoryTypeController(IMasterService<TzebB2bInventoryType> service)
        {
            _service = service;
        }

        [HttpGet("GetAll")]
        public async Task<ApiResponse<IEnumerable<TzebB2bInventoryType>>> GetAll()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet("GetById/{id}")]
        public async Task<ApiResponse<TzebB2bInventoryType>> GetById(int id)
        {
            return await _service.GetByIdAsync(id);
        }

        [HttpPost("Create")]
        public async Task<ApiResponse<TzebB2bInventoryType>> Create([FromBody] TzebB2bInventoryType item)
        {
            return await _service.AddAsync(item);
        }

        [HttpPut("Update")]
        public async Task<ApiResponse<TzebB2bInventoryType>> Update([FromBody] TzebB2bInventoryType item)
        {
            return await _service.UpdateAsync(item);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ApiResponse<bool>> Delete(int id)
        {
            return await _service.DeleteByIdAsync(id);
        }
    }
}
