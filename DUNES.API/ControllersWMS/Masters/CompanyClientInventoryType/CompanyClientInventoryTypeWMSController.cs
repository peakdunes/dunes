using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Masters.CompanyClientInventoryType;
using DUNES.API.ServicesWMS.Masters.CompanyClientItemStatus;
using DUNES.Shared.DTOs.WMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.CompanyClientInventoryType
{

    /// <summary>
    /// Manages client-level InventoryType mappings (enable/disable per client)
    /// using tenant scope from token (CompanyId / CompanyClientId).
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/wms/masters/company-client/type-inventory")]
    public class CompanyClientInventoryTypeWMSController : BaseController
    {
        private readonly ICompanyClientInventoryTypeWMSAPIService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyClientInventoryTypeWMSController"/> class.
        /// </summary>
        /// <param name="service">Service for CompanyClientInventoryType business logic.</param>
        public CompanyClientInventoryTypeWMSController(ICompanyClientInventoryTypeWMSAPIService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets all InventoryType mappings for the current tenant scope (company + client),
        /// including active and inactive mappings.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Standard API response with the list of mappings.</returns>
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return await HandleApi(ct =>
                 _service.GetAllAsync(CurrentCompanyId, CurrentCompanyClientId, ct),ct);

           

        }

        /// <summary>
        /// Gets a specific InventoryType mapping by mapping Id within the current tenant scope.
        /// </summary>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Standard API response with the mapping if found.</returns>
        [HttpGet("GetById/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.GetByIdAsync(id, CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        }

        /// <summary>
        /// Creates a new InventoryType mapping for the current tenant scope.
        /// CompanyId and CompanyClientId are always taken from token.
        /// </summary>
        /// <param name="request">Create DTO (tenant values are not accepted in body).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Standard API response with the created mapping.</returns>
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create(
            [FromBody] WMSCompanyClientInventoryTypeCreateDTO request,
            CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.CreateAsync(request, CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        }

        ///// <summary>
        ///// Updates an existing InventoryType mapping within the current tenant scope.
        ///// CompanyId and CompanyClientId are always taken from token.
        ///// </summary>
        ///// <param name="request">Update DTO.</param>
        ///// <param name="ct">Cancellation token.</param>
        ///// <returns>Standard API response with the updated mapping.</returns>
        //[HttpPut("Update")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status409Conflict)]
        //public async Task<IActionResult> Update(
        //    [FromBody] WMSCompanyClientInventoryTypeUpdateDTO request,
        //    CancellationToken ct)
        //{
        //    return await HandleApi(ct =>
        //        _service.UpdateAsync(request, CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        //}

        /// <summary>
        /// Deletes an InventoryType mapping by mapping Id within the current tenant scope.
        /// </summary>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Standard API response indicating delete result.</returns>
        [HttpDelete("Delete/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            return await HandleApi(ct =>
                 _service.DeleteAsync(id, CurrentCompanyId, CurrentCompanyClientId, ct),ct);
        }


        /// <summary>
        /// Activates or deactivates an InventoryType mapping within the current tenant scope.
        /// CompanyId and CompanyClientId are always taken from token.
        /// </summary>
        /// <param name="request">Set-active request DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Standard API response with the updated mapping.</returns>
        [HttpPatch("SetActive")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetActive(
            [FromBody] WMSCompanyClientInventoryTypeSetActiveDTO request,
            CancellationToken ct)
        {
            return await HandleApi(ct =>
                _service.SetActiveAsync(request, CurrentCompanyId, CurrentCompanyClientId, ct), ct);
        }
    }
}