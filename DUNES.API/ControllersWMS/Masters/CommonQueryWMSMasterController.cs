using DUNES.API.ModelsWMS.Masters;
using DUNES.API.ServicesWMS.Masters;
using DUNES.API.Utils.Responses;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Masters
{

    /// <summary>
    /// All WMS Master queries
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CommonQueryWMSMasterController : ControllerBase
    {

        private readonly ICommonQueryWMSMasterService _commonQueryWMSMasterService;


        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="commonQueryWMSMasterService"></param>
        public CommonQueryWMSMasterController(ICommonQueryWMSMasterService commonQueryWMSMasterService)
        {
            _commonQueryWMSMasterService = commonQueryWMSMasterService;
        }
        /// <summary>
        /// Get all company information for id
        /// </summary>
        /// <param name="companyid"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("company-information/{companyid}")]
        public async Task<IActionResult> GetCompanyInformation(int companyid)
        {

            var response = await _commonQueryWMSMasterService.GetCompanyInformation(companyid);

            return StatusCode(response.StatusCode, response);

        }

        /// <summary>
        /// Get all locations for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Locations>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("company-locations/{companyid}")]
        public async Task<IActionResult> GetAllLocationsByCompany(int companyid)
        {

            var response = await _commonQueryWMSMasterService.GetAllLocationsByCompany(companyid);

            return StatusCode(response.StatusCode, response);

        }

        /// <summary>
        /// Get All Inventory Types for a company client 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        [HttpGet("companyClient-inventoryTypes/{companyid}/{companyClient}")]
        public async Task<IActionResult> GetAllLocationsByCompany(int companyid, string companyClient )
        {

            var response = await _commonQueryWMSMasterService.GetAllInventoryTypesByCompanyClient(companyid, companyClient);

            return StatusCode(response.StatusCode, response);

        }

        /// <summary>
        /// Get All Active Inventory Types for a company client 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        [HttpGet("companyClient-active-inventoryTypes/{companyid}/{companyClient}")]
        public async Task<IActionResult> GetAllActiveInventoryTypesByCompanyClient(int companyid, string companyClient)
        {

            var response = await _commonQueryWMSMasterService.GetAllActiveInventoryTypesByCompanyClient (companyid, companyClient);

            return StatusCode(response.StatusCode, response);

        }

        /// <summary>
        /// Get All Item Status for a company client 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        [HttpGet("companyClient-itemStatus/{companyid}/{companyClient}")]
        public async Task<IActionResult> GetAllItemStatusByCompanyClient(int companyid, string companyClient)
        {

            var response = await _commonQueryWMSMasterService.GetAllItemStatusByCompanyClient (companyid, companyClient);

            return StatusCode(response.StatusCode, response);

        }

        /// <summary>
        /// Get All Active Item Status for a company client 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        [HttpGet("companyClient-active-itemStatus/{companyid}/{companyClient}")]
        public async Task<IActionResult> GetAllActiveItemStatusByCompanyClient(int companyid, string companyClient)
        {

            var response = await _commonQueryWMSMasterService.GetAllActiveItemStatusByCompanyClient(companyid, companyClient);

            return StatusCode(response.StatusCode, response);

        }


        /// <summary>
        /// Get All Racks for a company client 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        [HttpGet("companyClient-racks/{companyid}/{companyClient}")]
        public async Task<IActionResult> GetAllRacksByCompanyClient(int companyid, string companyClient)
        {

            var response = await _commonQueryWMSMasterService.GetAllRacksByCompanyClient(companyid, companyClient);

            return StatusCode(response.StatusCode, response);

        }


        /// <summary>
        /// Get All Active Racks for a company client 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        [HttpGet("companyClient-active-racks/{companyid}/{companyClient}")]
        public async Task<IActionResult> GetAllActiveRacksByCompanyClient(int companyid, string companyClient)
        {

            var response = await _commonQueryWMSMasterService.GetAllActiveRacksByCompanyClient(companyid, companyClient);

            return StatusCode(response.StatusCode, response);

        }


        /// <summary>
        /// Get All Bins for a company client 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        [HttpGet("companyClient-bins/{companyid}/{companyClient}")]
        public async Task<IActionResult> GetAllBinsByCompanyClient(int companyid, string companyClient)
        {

            var response = await _commonQueryWMSMasterService.GetAllBinsByCompanyClient(companyid, companyClient);

            return StatusCode(response.StatusCode, response);

        }

        /// <summary>
        /// Get All Active Bins for a company client 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        [HttpGet("companyClient-active-bins/{companyid}/{companyClient}")]
        public async Task<IActionResult> GetAllActiveBinsByCompanyClient(int companyid, string companyClient)
        {

            var response = await _commonQueryWMSMasterService.GetAllActiveBinsByCompanyClient(companyid, companyClient);

            return StatusCode(response.StatusCode, response);

        }
       
    }
}
