using APIZEBRA.Models.Masters;
using APIZEBRA.Services.Masters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIZEBRA.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TrepairActionsCodesController : ControllerBase
    {

        private readonly IMasterService<TrepairActionsCodes> _service;

        public TrepairActionsCodesController(IMasterService<TrepairActionsCodes> service)
        {
            _service = service;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TrepairActionsCodes item)
        {
            var created = await _service.AddAsync(item);
            return Ok(created);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TrepairActionsCodes item)
        {
            var updated = await _service.UpdateAsync(item);
            return Ok(updated);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteByIdAsync(id);
            return result ? Ok() : NotFound();
        }


      
    }
}
