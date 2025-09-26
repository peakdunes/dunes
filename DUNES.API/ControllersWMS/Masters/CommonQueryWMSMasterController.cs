using DUNES.API.Controllers;
using DUNES.API.ModelsWMS.Masters;
using DUNES.API.ServicesWMS.Masters;
using DUNES.API.Utils.Responses;
using DUNES.Shared.DTOs.WMS;
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
    public class CommonQueryWMSMasterController : BaseController
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
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("company-information/{companyid}")]
        public async Task<IActionResult> GetCompanyInformation(int companyid, CancellationToken ct)
        {
            return await HandleApi(ct => _commonQueryWMSMasterService.GetCompanyInformation(companyid, ct), ct);
           

        }

        /// <summary>
        /// Get all locations for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<WMSLocationsDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("company-locations/{companyid}")]
        public async Task<IActionResult> GetAllActiveLocationsByCompany(int companyid, CancellationToken ct)
        {
            return await HandleApi(ct => _commonQueryWMSMasterService.GetAllActiveLocationsByCompany(companyid, ct), ct);
        }

        /// <summary>
        /// Get All active company clients by location id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="locationid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<WMSLocationsDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("companyclients-active-by-locations/{companyid}/{locationid}")]
        public async Task<IActionResult> GetAllActiveClientCompaniesByLocation(int companyid,int locationid, CancellationToken ct)
        {
            return await HandleApi(ct => _commonQueryWMSMasterService.GetAllActiveClientCompaniesByLocation(companyid, locationid, ct), ct);
        }


        /// <summary>
        /// Get All Inventory Types for a company client 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("companyClient-inventoryTypes/{companyid}/{companyClient}")]
        public async Task<IActionResult> GetAllInventoryTypesByCompanyClient(int companyid, string companyClient, CancellationToken ct )
        {

            return await HandleApi(ct => _commonQueryWMSMasterService.GetAllInventoryTypesByCompanyClient(companyid, companyClient, ct), ct);
        }

       

       
        /// <summary>
        /// Get All Active Inventory Types for a company client 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("companyClient-active-inventoryTypes/{companyid}/{companyClient}")]
        public async Task<IActionResult> GetAllActiveInventoryTypesByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {
            return await HandleApi(ct => _commonQueryWMSMasterService.GetAllActiveInventoryTypesByCompanyClient(companyid, companyClient, ct), ct);

        }

        /// <summary>
        /// Get All Item Status for a company client 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("companyClient-itemStatus/{companyid}/{companyClient}")]
        public async Task<IActionResult> GetAllItemStatusByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {

            return await HandleApi(ct => _commonQueryWMSMasterService.GetAllItemStatusByCompanyClient(companyid, companyClient, ct), ct);

        }

        /// <summary>
        /// Get All Active Item Status for a company client 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("companyClient-active-itemStatus/{companyid}/{companyClient}")]
        public async Task<IActionResult> GetAllActiveItemStatusByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {

            return await HandleApi(ct => _commonQueryWMSMasterService.GetAllActiveItemStatusByCompanyClient(companyid, companyClient, ct), ct);

        }


        /// <summary>
        /// Get All Racks for a company client 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("companyClient-racks/{companyid}/{companyClient}")]
        public async Task<IActionResult> GetAllRacksByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {

            return await HandleApi(ct => _commonQueryWMSMasterService.GetAllRacksByCompanyClient(companyid, companyClient, ct), ct);

        }


        /// <summary>
        /// Get All Active Racks for a company client 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("companyClient-active-racks/{companyid}/{companyClient}")]
        public async Task<IActionResult> GetAllActiveRacksByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {

            return await HandleApi(ct => _commonQueryWMSMasterService.GetAllActiveRacksByCompanyClient(companyid, companyClient, ct), ct);

           

        }


        /// <summary>
        /// Get All Bins for a company client 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("companyClient-bins/{companyid}/{companyClient}")]
        public async Task<IActionResult> GetAllBinsByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {
            return await HandleApi(ct => _commonQueryWMSMasterService.GetAllBinsByCompanyClient(companyid, companyClient, ct), ct);

        }

        /// <summary>
        /// Get All Active Bins for a company client 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("companyClient-active-bins/{companyid}/{companyClient}")]
        public async Task<IActionResult> GetAllActiveBinsByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {
            return await HandleApi(ct => _commonQueryWMSMasterService.GetAllActiveBinsByCompanyClient(companyid, companyClient, ct), ct);

        }

        /// <summary>
        /// Get all Warehouse Organization for a Client Company
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("companyClient-warehouse-organization/{companyid}/{companyClient}")]
        public async Task<IActionResult> GetAllWareHouseOrganizationByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {
            return await HandleApi(ct => _commonQueryWMSMasterService.GetAllWareHouseOrganizationByCompanyClient(companyid, companyClient, ct), ct);

        }

        
    }
}
