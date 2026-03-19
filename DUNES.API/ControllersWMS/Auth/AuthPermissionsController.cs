using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Auth;
using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Utils.Reponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
