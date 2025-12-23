using DUNES.API.Controllers;
using DUNES.API.ModelsWMS.Masters;
using DUNES.API.ServicesWMS.Masters.CompaniesClientDivision;
using DUNES.Shared.DTOs.Masters;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.CompanyClientDivision
{
    /// <summary>
    /// Company client Division controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyClientDivisionWMSController : BaseController
    {

        private readonly ICompaniesClientDivisionWMSAPIService _service;

        /// <summary>
        /// contructor (DI)
        /// </summary>
        /// <param name="service"></param>
        public CompanyClientDivisionWMSController(ICompaniesClientDivisionWMSAPIService service)
        {
            _service = service;
        }

        /// <summary>
        /// Return all company divisions
        /// </summary>
        /// <param name="ct"></param>

        /// <returns></returns>
        [ProducesResponseType(typeof(List<WMSCompanyClientDivisionReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("all-client-divisions")]

        public async Task<IActionResult> GetAllCompaniesClientDivisionInformationAsync(CancellationToken ct)
        {

            return await HandleApi(ct => _service.GetAllCompaniesClientDivisionInformation(ct), ct);


        }



        /// <summary>
        /// Return all company divisions for company client id
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="companyclientid"></param>

        /// <returns></returns>
        [ProducesResponseType(typeof(List<WMSCompanyClientDivisionReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("all-client-company-divisions-by-company/{companyclientid}")]

        public async Task<IActionResult> GetAllCompaniesClientDivisionInformationByCompanyClient(int companyclientid, CancellationToken ct)
        {

            return await HandleApi(ct => _service.GetAllCompaniesClientDivisionInformationByCompanyClient(companyclientid,ct), ct);


        }


        /// <summary>
        /// Return client company division by identification
        /// </summary>
        /// <param name="divisionid"></param>
        ///  <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<WMSCompanyClientDivisionReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("wms-client-company-division-by-identification/{divisionid}")]

        public async Task<IActionResult> GetCompanyClientDivisionByIdAsync(int divisionid, CancellationToken ct)
        {

            return await HandleApi(ct => _service.GetCompanyClientDivisionById(divisionid, ct), ct);

        }


        /// <summary>
        /// get company client division information by name
        /// </summary>
        /// <param name="companyClientId"></param>
        /// <param name="divisionname"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<WMSCompanyClientDivisionReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("wms-client-company-division-by-name/{companyClientId}/{divisionname}")]

        public async Task<IActionResult> GetCompanyClientDivisionByNameAsync(int companyClientId, string divisionname,  CancellationToken ct)
        {

            return await HandleApi(ct => _service.GetCompanyClientDivisionByNameAsync(companyClientId, divisionname, ct), ct);

        }

     


        /// <summary>
        /// Create a new Client Company Division
        /// </summary>
        /// <param name="companyinfo"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [HttpPost("wms-create-client-company-division")]
        public async Task<IActionResult> AddClientCompanyDivisionAsync([FromBody] WMSCompanyClientDivisionDTO companyinfo, CancellationToken ct)
        {
            return await HandleApi(ct => _service.AddClientCompanyDivisionAsync(companyinfo, ct), ct);
        }


        /// <summary>
        /// UPdate Client Company Division
        /// </summary>
        /// <param name="companyinfo"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [HttpPost("wms-update-client-company-division")]
        public async Task<IActionResult> UpdateClientCompanyDivisionAsync([FromBody] WMSCompanyClientDivisionDTO companyinfo, CancellationToken ct)
        {
            return await HandleApi(ct => _service.UpdateClientCompanyDivisionAsync(companyinfo, ct), ct);
        }

    }
}
