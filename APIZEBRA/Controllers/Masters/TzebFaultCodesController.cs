using APIZEBRA.Models.Masters;
using APIZEBRA.Services.Masters;
using APIZEBRA.Utils.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIZEBRA.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class TzebFaultCodesController : ControllerBase
    {
        private readonly IMasterService<TzebFaultCodes> _service;


        public TzebFaultCodesController(IMasterService<TzebFaultCodes> service)
        {
            _service = service;
        }


        [HttpGet("GetAll")]
        public async Task<ApiResponse<IEnumerable<TzebFaultCodes>>> GetAll()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet("GetById/{id}")]
        public async Task<ApiResponse<TzebFaultCodes>> GetById(int id)
        {
            return await _service.GetByIdAsync(id);
        }

        [HttpPost("Create")]
        public async Task<ApiResponse<TzebFaultCodes>> Create([FromBody] TzebFaultCodes item)
        {
            return await _service.AddAsync(item);
        }

        [HttpPut("Update")]
        public async Task<ApiResponse<TzebFaultCodes>> Update([FromBody] TzebFaultCodes item)
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
