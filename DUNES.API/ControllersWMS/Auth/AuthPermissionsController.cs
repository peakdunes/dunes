using DUNES.API.ServicesWMS.Auth;
using DUNES.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Auth
{
    /// <summary>
    /// Provides endpoints to manage the permission catalog.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthPermissionController : ControllerBase
    {
        private readonly IAuthPermissionService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthPermissionController"/> class.
        /// </summary>
        /// <param name="service">Permission catalog service.</param>
        public AuthPermissionController(IAuthPermissionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Returns all permissions from the catalog.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of permissions.</returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var response = await _service.GetAllAsync(ct);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Returns a permission by its identifier.
        /// </summary>
        /// <param name="id">Permission identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Permission information.</returns>
        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var response = await _service.GetByIdAsync(id, ct);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Returns all permissions that belong to a specific functional group and module.
        /// This endpoint returns the complete permission catalog for the requested module.
        /// </summary>
        /// <param name="groupName">Functional group name. Example: Masters, Auth, Reports.</param>
        /// <param name="moduleName">Module name. Example: Locations, Users, CompanyClientItemStatus.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of permissions for the requested module.</returns>
        [HttpGet("GetByModule/{groupName}/{moduleName}")]
        public async Task<IActionResult> GetByModule(string groupName, string moduleName, CancellationToken ct)
        {
            var response = await _service.GetByModuleAsync(groupName, moduleName, ct);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Returns active permissions for a specific functional group and module
        /// that are configured to be rendered as row-level actions in index tables.
        /// Example: Edit, Delete, ResetPassword, Deactivate.
        /// </summary>
        /// <param name="groupName">Functional group name. Example: Masters, Auth, Reports.</param>
        /// <param name="moduleName">Module name. Example: Locations, Users, CompanyClientItemStatus.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of row-action permissions for the requested module.</returns>
        [HttpGet("GetRowActionsByModule/{groupName}/{moduleName}")]
        public async Task<IActionResult> GetRowActionsByModule(string groupName, string moduleName, CancellationToken ct)
        {
            var response = await _service.GetRowActionsByModuleAsync(groupName, moduleName, ct);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Returns active permissions for a specific functional group and module
        /// that are configured to be rendered as toolbar or header actions in index views.
        /// Example: Create, Export, Process.
        /// </summary>
        /// <param name="groupName">Functional group name. Example: Masters, Auth, Reports.</param>
        /// <param name="moduleName">Module name. Example: Locations, Users, CompanyClientItemStatus.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of toolbar-action permissions for the requested module.</returns>
        [HttpGet("GetToolbarActionsByModule/{groupName}/{moduleName}")]
        public async Task<IActionResult> GetToolbarActionsByModule(string groupName, string moduleName, CancellationToken ct)
        {
            var response = await _service.GetToolbarActionsByModuleAsync(groupName, moduleName, ct);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Creates a new permission in the catalog.
        /// </summary>
        /// <param name="dto">Permission creation DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Created permission information.</returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] AuthPermissionCreateDTO dto, CancellationToken ct)
        {
            var response = await _service.CreateAsync(dto, ct);
            return StatusCode(response.StatusCode, response);
        }
    }
}