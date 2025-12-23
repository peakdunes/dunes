using DUNES.API.Controllers;
using DUNES.API.ModelsWMS.Masters;
using DUNES.API.ServicesWMS.Masters.ClientCompanies;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.DTOs.Masters;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DUNES.API.ControllersWMS.Masters.ClientCompanies
{

    /// <summary>
    /// Client companies controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientCompaniesWMSController : BaseController
    {



        private readonly IClientCompaniesWMSAPIService _service;

        /// <summary>
        /// contructor dependency injection
        /// </summary>
        /// <param name="service"></param>
        public ClientCompaniesWMSController(IClientCompaniesWMSAPIService service)
        {
            
            _service = service;
        }

        /// <summary>
        /// Return all company clients
        /// </summary>
        /// <param name="ct"></param>
       
        /// <returns></returns>
        [ProducesResponseType(typeof(List<WMSClientCompaniesReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("all-client-companies")]

        public async Task<IActionResult> GetAllClientCompaniesInformationAsync( CancellationToken ct)
        {

            return await HandleApi(ct => _service.GetAllClientCompaniesInformation(ct), ct);


        }

        /// <summary>
        /// Return client company by identification 
        /// </summary>
        /// <param name="companyid"></param>
        ///  <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<WMSClientCompaniesReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("wms-client-company-by-identification/{companyid}")]

        public async Task<IActionResult> GetClientCompanyInformationByIdentificationAsync(string companyid, CancellationToken ct)
        {

            return await HandleApi(ct => _service.GetClientCompanyInformationByIdentificationAsync(companyid, ct), ct);

        }


        /// <summary>
        /// get company client information by name
        /// </summary>
        /// <param name="companyname"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<WMSClientCompaniesReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("wms-client-company-by-name/{companyname}")]

        public async Task<IActionResult> GetClientCompanyInformationByNameAsync(string companyname, CancellationToken ct)
        {

            return await HandleApi(ct => _service.GetClientCompanyInformationByNameAsync(companyname, ct), ct);

        }

        /// <summary>
        /// Return client company by identification  bins for a id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<WMSClientCompaniesReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("wms-client-company-by-id/{id}")]

        public async Task<IActionResult> GetClientCompanyInformationByIdentificationAsync(int id, CancellationToken ct)
        {

            return await HandleApi(ct => _service.GetClientCompanyInformationByIdAsync(id, ct), ct);

        }


        /// <summary>
        /// Create a new Client Company
        /// </summary>
        /// <param name="companyinfo"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [HttpPost("wms-create-client-company")]
        public async Task<IActionResult> CreateClientCompany([FromBody] WMSClientCompaniesDTO companyinfo, CancellationToken ct)
        {
            return await HandleApi(ct => _service.AddClientCompanyAsync(companyinfo, ct), ct);
        }

    }
}
