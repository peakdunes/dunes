using APIZEBRA.Models.Masters;
using APIZEBRA.Services.Masters;
using APIZEBRA.Utils.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIZEBRA.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class TzebWorkCodesTargetsController : ControllerBase
    {
        private readonly IMasterService<TzebWorkCodesTargets> _service;


        public TzebWorkCodesTargetsController(IMasterService<TzebWorkCodesTargets> service)
        {
            _service = service;
        }



        [HttpGet("GetAll")]
        public async Task<ApiResponse<IEnumerable<TzebWorkCodesTargets>>> GetAll()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet("GetById/{id}")]
        public async Task<ApiResponse<TzebWorkCodesTargets>> GetById(int id)
        {
            return await _service.GetByIdAsync(id);
        }

        [HttpPost("Create")]
        public async Task<ApiResponse<TzebWorkCodesTargets>> Create([FromBody] TzebWorkCodesTargets item)
        {
            return await _service.AddAsync(item);
        }

        [HttpPut("Update")]
        public async Task<ApiResponse<TzebWorkCodesTargets>> Update([FromBody] TzebWorkCodesTargets item)
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
