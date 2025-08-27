using DUNES.API.Controllers;
using DUNES.API.ModelsWMS.Masters;
using DUNES.API.ModelsWMS.Transactions;
using DUNES.API.ServicesWMS.Inventory.Common.Queries;
using DUNES.API.Utils.Responses;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
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


        /// <summary>
        /// get all active transactions concepts
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Transactionconcepts>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("active-transaction-concepts/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllActiveTransactionsConcept(int companyid, string companyClient)
        {

            var response = await _service.GetAllActiveTransactionsConcept(companyid, companyClient);

            return StatusCode(response.StatusCode, response);

        }

        /// <summary>
        /// get all transactions concepts
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Transactionconcepts>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("transaction-concepts/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllTransactionsConcept(int companyid, string companyClient)
        {

            var response = await _service.GetAllTransactionsConcept(companyid, companyClient);

            return StatusCode(response.StatusCode, response);

        }


        /// <summary>
        /// get all active input transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Transactiontypes>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("active-input-transaction/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllActiveTransactionsInputType(int companyid, string companyClient)
        {
            var response = await _service.GetAllActiveTransactionsInputType(companyid, companyClient);

            return StatusCode(response.StatusCode, response);

        }


        /// <summary>
        /// get all input transactions type
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Transactiontypes>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("input-transaction/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllTransactionsInputType(int companyid, string companyClient)
        {
            var response = await _service.GetAllActiveTransactionsInputType(companyid, companyClient);

            return StatusCode(response.StatusCode, response);

        }


        /// <summary>
        /// get all active output transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Transactiontypes>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("active-output-transaction/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllActiveTransactionsOutputType(int companyid, string companyClient)
        {
            var response = await _service.GetAllActiveTransactionsOutputType(companyid, companyClient);

            return StatusCode(response.StatusCode, response);

        }


        /// <summary>
        /// get all output transactions type
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Transactiontypes>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("output-transaction/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllTransactionsOutputType(int companyid, string companyClient)
        {
            var response = await _service.GetAllActiveTransactionsOutputType(companyid, companyClient);

            return StatusCode(response.StatusCode, response);

        }


        /// <summary>
        /// Get all active Inventory Types for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Transactionconcepts>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("active-inventorytype/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllActiveInventoryType(int companyid, string companyClient)
        {

            var response = await _service.GetAllActiveInventoryType(companyid, companyClient);

            return StatusCode(response.StatusCode, response);

        }

        /// <summary>
        /// Get all Inventory Types for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Transactionconcepts>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("inventorytype/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllInventoryType(int companyid, string companyClient)
        {

            var response = await _service.GetAllInventoryType(companyid, companyClient);

            return StatusCode(response.StatusCode, response);

        }



        /// <summary>
        /// Get all active item status for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Itemstatus>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("active-itemstatus/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllActiveItemStatus(int companyid, string companyClient)
        {

            var response = await _service.GetAllActiveItemStatus(companyid, companyClient);

            return StatusCode(response.StatusCode, response);

        }




        /// <summary>
        /// Get all item status for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Itemstatus>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("itemstatus/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllItemStatus(int companyid, string companyClient)
        {

            var response = await _service.GetAllItemStatus(companyid, companyClient);

            return StatusCode(response.StatusCode, response);

        }

        /// <summary>
        /// Get current inventory for a client company part number
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="partnumber"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<WMSInventoryDetailByPartNumberDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("inventoryByPartNumber/{companyid}/{companyClient}/{partnumber}")]

        public async Task<IActionResult> GetInventoryByItem(int companyid, string companyClient, string partnumber)
        {

            var response = await _service.GetInventoryByItem(companyid, companyClient, partnumber);

            return StatusCode(response.StatusCode, response);

        }


        /// <summary>
        /// Get current inventory for a client company part number
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="partnumber"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Itemsbybin>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("itemBinDistribution/{companyid}/{companyClient}/{partnumber}")]

        public async Task<IActionResult> GetItemBinsDistribution(int companyid, string companyClient, string partnumber)
        {

            var response = await _service.GetItemBinsDistribution(companyid, companyClient, partnumber);

            return StatusCode(response.StatusCode, response);

        }

        
    }
}
