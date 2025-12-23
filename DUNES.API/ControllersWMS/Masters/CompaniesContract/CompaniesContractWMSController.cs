using DUNES.API.Controllers;
using DUNES.API.ServicesWMS.Masters.CompaniesContract;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters.CompaniesContract
{
    /// <summary>
    /// company client contracts controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompaniesContractWMSController : BaseController
    {

        private readonly ICompaniesContractWMSAPIService _service;

        /// <summary>
        /// DI
        /// </summary>
        /// <param name="service"></param>
        public CompaniesContractWMSController(ICompaniesContractWMSAPIService service)
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
        [HttpGet("all-client-contracts")]

        public async Task<IActionResult> GetAllCompaniesClientContractsInformationAsync(CancellationToken ct)
        {

            return await HandleApi(ct => _service.GetAllClientCompaniesContractInformationAsync(ct), ct);


        }



        /// <summary>
        /// Return all company divisions for company client id
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="companyclientid"></param>

        /// <returns></returns>
        [ProducesResponseType(typeof(List<WMSCompaniesContractReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("all-client-company-contract-by-company/{companyclientid}")]

        public async Task<IActionResult> GetAllCompaniesClientContractInformationByCompanyClient(int companyclientid, CancellationToken ct)
        {

            return await HandleApi(ct => _service.GetClientCompanyInformationContractByCompanyIdAsync(companyclientid, ct), ct);


        }


        /// <summary>
        /// Return client company division by identification
        /// </summary>
        /// <param name="contractid"></param>
        ///  <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<WMSCompaniesContractReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("wms-client-company-contract-by-identification/{contractid}")]

        public async Task<IActionResult> GetCompanyClientDivisionByIdAsync(int contractid, CancellationToken ct)
        {

            return await HandleApi(ct => _service.GetClientCompanyContractInformationByIdAsync(contractid, ct), ct);

        }


        /// <summary>
        /// get company client contract information by contract number and company
        /// </summary>
        /// <param name="companyClientId"></param>
        /// <param name="contractnumber"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<WMSCompaniesContractReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("wms-client-company-contract-by-company-number/{companyClientId}/{contractnumber}")]

        public async Task<IActionResult> GetCompanyClientContractByNumberCompanyIdAsync(int companyClientId, string contractnumber, CancellationToken ct)
        {

            return await HandleApi(ct => _service.GetClientCompanyInformationContractByNumberCompanyIdAsync(companyClientId, contractnumber, ct), ct);

        }

        /// <summary>
        /// get company client contract information by contract number 
        /// </summary>
        /// <param name="contractnumber"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<WMSCompaniesContractReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("wms-client-company-contract-by-number/{contractnumber}")]

        public async Task<IActionResult> GetCompanyClientContractByNumberAsync( string contractnumber, CancellationToken ct)
        {

            return await HandleApi(ct => _service.GetClientCompanyInformationContractByNumberAsync(contractnumber, ct), ct);

        }


        /// <summary>
        /// Create a new Client Company Division
        /// </summary>
        /// <param name="companyinfo"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [HttpPost("wms-create-client-company-contract")]
        public async Task<IActionResult> AddClientCompanyContractAsync([FromBody] WMSCompaniesContractDTO companyinfo, CancellationToken ct)
        {


            return await HandleApi(ct => _service.AddClientCompanyContractAsync(companyinfo, ct), ct);
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
        public async Task<IActionResult> UpdateClientCompanyDivisionAsync([FromBody] WMSCompaniesContractDTO companyinfo, CancellationToken ct)
        {
            return await HandleApi(ct => _service.UpdateClientCompanyContractAsync(companyinfo, ct), ct);
        }


    }
}
