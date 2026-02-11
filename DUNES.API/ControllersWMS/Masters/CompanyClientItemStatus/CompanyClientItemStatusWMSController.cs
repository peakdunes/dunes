using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Masters.CompanyClientInventoryCategory;
using DUNES.API.ServicesWMS.Masters.CompanyClientItemStatus;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.CompanyClientItemStatus
{
    /// <summary>
    /// Controlador para la gestión de estados de ítems específicos por cliente.
    /// </summary>
    [ApiController]
    [Route("api/wms/masters/company-client/item-statuses")]
    public class CompanyClientItemStatusWMSController : BaseController
    {
        private readonly ICompanyClientItemStatusService _service;

        /// <summary>
        /// constructor(DI)
        /// </summary>
        /// <param name="service"></param>
        public CompanyClientItemStatusWMSController(ICompanyClientItemStatusService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene todos los estados de ítems del cliente actual.
        /// </summary>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetAllAsync(CurrentCompanyId, CurrentCompanyClientId, ct),
                ct);
        }

        /// <summary>
        /// Obtiene un estado de ítem específico por ID.
        /// </summary>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.GetByIdAsync(CurrentCompanyId, CurrentCompanyClientId, id, ct),
                ct);
        }

        /// <summary>
        /// Crea una nueva asignación de estado de ítem para el cliente actual.
        /// </summary>
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] WMSCompanyClientItemStatusCreateDTO dto, CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.CreateAsync(CurrentCompanyId, CurrentCompanyClientId, dto, ct),
                ct);
        }

        /// <summary>
        /// Actualiza una asignación existente de estado de ítem.
        /// </summary>
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] WMSCompanyClientItemStatusUpdateDTO dto, CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.UpdateAsync(CurrentCompanyId, CurrentCompanyClientId, dto, ct),
                ct);
        }

        /// <summary>
        /// Activa o desactiva un estado de ítem asignado al cliente.
        /// </summary>
        [HttpPut("SetActive/{id}")]
        public async Task<IActionResult> SetActive(int id, [FromQuery] bool isActive, CancellationToken ct)
        {
            return await HandleApi(
                ct => _service.SetActiveAsync(CurrentCompanyId, CurrentCompanyClientId, id, isActive, ct),
                ct);
        }
    }
}
