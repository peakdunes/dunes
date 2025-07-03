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
    public class TzebB2bInventoryTypeController : BaseController
    {

        private readonly IMasterService<TzebB2bInventoryType> _service;

        public TzebB2bInventoryTypeController(IMasterService<TzebB2bInventoryType> service)
        {
            _service = service;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return await Handle(async () =>
            {
                var result = await _service.GetAllAsync();
                return Ok(new { success = true, mensaje = "Consulta exitosa.", data = result });
            });
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return await Handle(async () =>
            {
                if (id <= 0)
                    return BadRequest(new { success = false, mensaje = "ID inválido.", data = (object)null });

                var result = await _service.GetByIdAsync(id);
                if (result == null)
                    return NotFound(new { success = false, mensaje = "No se encontró el registro.", data = (object)null });

                return Ok(new { success = true, mensaje = "Consulta exitosa.", data = result });
            });
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TzebB2bInventoryType model)
        {
            return await Handle(async () =>
            {
                var validation = ValidateModel(model);
                if (!validation.IsValid)
                    return BadRequest(new { success = false, mensaje = validation.ErrorMessage, data = (object)null });

               
                var result = await _service.AddAsync(model);

                return Ok(new { success = true, mensaje = "Creado correctamente", data = result });
            });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TzebB2bInventoryType model)
        {
            return await Handle(async () =>
            {
                var validation = ValidateModel(model);
                if (!validation.IsValid)
                    return BadRequest(new
                    {
                        success = false,
                        mensaje = validation.ErrorMessage,
                        data = (object)null
                    });

                var result = await _service.UpdateAsync(model);

                return Ok(new
                {
                    success = true,
                    mensaje = "Actualizado correctamente",
                    data = result
                });
            });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await Handle(async () =>
            {
                if (id <= 0)
                    return BadRequest(new { success = false, mensaje = "ID inválido.", data = (object)null });

                var deleted = await _service.DeleteByIdAsync(id);
                if (!deleted)
                    return NotFound(new { success = false, mensaje = "No se encontró el registro para eliminar.", data = (object)null });

                return Ok(new { success = true, mensaje = "Eliminado correctamente.", data = (object)null });
            });
        }

        private (bool IsValid, string ErrorMessage) ValidateModel(TzebB2bInventoryType model)
        {
            if (string.IsNullOrWhiteSpace(model.Description))
                return (false, "El campo 'Description' es obligatorio.");
            if (model.Description.Length > 100)
                return (false, "El campo 'Description' no puede superar los 100 caracteres.");
            return (true, string.Empty);
        }
    }
}
