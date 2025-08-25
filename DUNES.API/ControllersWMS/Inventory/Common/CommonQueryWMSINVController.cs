using DUNES.API.Controllers;
using DUNES.API.ModelsWMS.Masters;
using DUNES.API.ServicesWMS.Inventory.Common.Queries;
using DUNES.API.Utils.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Inventory.Common
{


    /// <summary>
    /// Common WMS queries
    /// </summary>
    /// 
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommonQueryWMSINVController : BaseController
    {


        private readonly ICommonQueryWMSINVService _service;

        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="service"></param>
        public CommonQueryWMSINVController(ICommonQueryWMSINVService service)
        {

            _service = service;

        }


        /// <summary>
        /// Return all active bins for a company client
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Bines>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("wms-act-bins/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllActiveBinsByCompanyClient(int companyid, string companyClient)
        {
            
            var response = await _service.GetAllActiveBinsByCompanyClient(companyid, companyClient);

            return StatusCode(response.StatusCode, response);

        }



        /// <summary>
        /// Return all  bins for a company client
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Bines>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("wms-bins/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllBinsByCompanyClient(int companyid, string companyClient)
        {

          var response = await _service.GetAllBinsByCompanyClient(companyid, companyClient);

          return StatusCode(response.StatusCode, response);

        }
    }
}
