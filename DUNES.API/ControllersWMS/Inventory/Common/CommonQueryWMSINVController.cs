using DUNES.API.Controllers;
using DUNES.API.ModelsWMS.Masters;
using DUNES.API.ModelsWMS.Transactions;
using DUNES.API.ServicesWMS.Inventory.Common.Queries;

using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

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

        public async Task<IActionResult> GetAllActiveBinsByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {

            return await HandleApi(ct => _service.GetAllActiveBinsByCompanyClient(companyid, companyClient, ct), ct);
                    

        }

        /// <summary>
        /// Return all  bins for a company client
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Bines>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("wms-bins/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllBinsByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {

            return await HandleApi(ct => _service.GetAllBinsByCompanyClient(companyid, companyClient, ct), ct);


          

        }


        /// <summary>
        /// get all active transactions concepts
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Transactionconcepts>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("active-transaction-concepts/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllActiveTransactionsConcept(int companyid, string companyClient, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetAllActiveTransactionsConcept(companyid, companyClient, ct), ct);
        }

        /// <summary>
        /// get all transactions concepts
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Transactionconcepts>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("transaction-concepts/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllTransactionsConcept(int companyid, string companyClient, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetAllTransactionsConcept(companyid, companyClient, ct), ct);

        }


        /// <summary>
        /// get all active input transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Transactiontypes>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("active-input-transaction/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllActiveTransactionsInputType(int companyid, string companyClient, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetAllActiveTransactionsInputType(companyid, companyClient, ct), ct);

        }

        /// <summary>
        /// Get All Active Input Type Transfer transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Transactiontypes>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("active-transfer-input-transaction/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllActiveTransferTransactionsInputType(int companyid, string companyClient, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetAllActiveTransferTransactionsInputType(companyid, companyClient, ct), ct);

        }

        /// <summary>
        /// Get All Input Type Transfer transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Transactiontypes>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("all-transfer-input-transaction/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllTransferTransactionsInputType(int companyid, string companyClient, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetAllTransferTransactionsInputType(companyid, companyClient, ct), ct);

        }


        /// <summary>
        /// Get All Active Output Type Transfer transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [ProducesResponseType(typeof(List<Transactiontypes>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("active-transfer-output-transaction/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllActiveTransferTransactionsOutputType(int companyid, string companyClient, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetAllActiveTransferTransactionsOutputType(companyid, companyClient, ct), ct);

        }


        /// <summary>
        /// Get All Output Type Transfer transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [ProducesResponseType(typeof(List<Transactiontypes>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("all-transfer-output-transaction/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllTransferTransactionsOutputType(int companyid, string companyClient, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetAllTransferTransactionsOutputType(companyid, companyClient, ct), ct);

        }

        /// <summary>
        /// Get one transaction type by ID
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [ProducesResponseType(typeof(Transactiontypes), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("transaction-by-id/{companyid}/{companyClient}/{id}")]

        public async Task<IActionResult> GetTransactionsTypeById(int companyid, string companyClient,int id, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetTransactionsTypeById(companyid, companyClient,id, ct), ct);

        }

        /// <summary>
        /// get all input transactions type
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Transactiontypes>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("input-transaction/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllTransactionsInputType(int companyid, string companyClient, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetAllActiveTransactionsInputType(companyid, companyClient, ct), ct);

        }


        /// <summary>
        /// get all active output transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Transactiontypes>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("active-output-transaction/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllActiveTransactionsOutputType(int companyid, string companyClient, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetAllActiveTransactionsOutputType(companyid, companyClient, ct), ct);

        }


        /// <summary>
        /// get all output transactions type
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Transactiontypes>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("output-transaction/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllTransactionsOutputType(int companyid, string companyClient, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetAllActiveTransactionsOutputType(companyid, companyClient, ct), ct);

        }


        /// <summary>
        /// Get all active Inventory Types for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Transactionconcepts>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("active-inventorytype/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllActiveInventoryType(int companyid, string companyClient, CancellationToken ct)
        {

            return await HandleApi(ct => _service.GetAllActiveInventoryType(companyid, companyClient, ct), ct);


        }

        /// <summary>
        /// Get all Inventory Types for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Transactionconcepts>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("inventorytype/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllInventoryType(int companyid, string companyClient, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetAllInventoryType(companyid, companyClient, ct), ct);

        }



        /// <summary>
        /// Get all active item status for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Itemstatus>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("active-itemstatus/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllActiveItemStatus(int companyid, string companyClient, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetAllActiveItemStatus(companyid, companyClient, ct), ct);

        }




        /// <summary>
        /// Get all item status for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Itemstatus>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("itemstatus/{companyid}/{companyClient}")]

        public async Task<IActionResult> GetAllItemStatus(int companyid, string companyClient, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetAllItemStatus(companyid, companyClient, ct), ct);

        }

        /// <summary>
        /// Get current inventory On-hand for a client company part number
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="partnumber"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<WMSInventoryDetailByPartNumberDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("onHand-InvByPartNumber/{companyid}/{companyClient}/{partnumber}")]

        public async Task<IActionResult> GetOnHandInventoryByItem(int companyid, string companyClient, string partnumber , CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetOnHandInventoryByItem(companyid, companyClient, partnumber, ct), ct);


        }
        /// <summary>
        /// Get current inventory On-hand for a client company, part number, inventory type
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="partnumber"></param>
        /// <param name="typeid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<WMSInventoryDetailByPartNumberDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("onHand-InvByPartNumber-Type/{companyid}/{companyClient}/{partnumber}/{typeid}")]
        public async Task<IActionResult> GetOnHandInventoryByItemInventoryType(int companyid, string companyClient, string partnumber, int typeid, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetOnHandInventoryByItemInventoryType(companyid, companyClient, partnumber, typeid, ct), ct);

        }


        /// <summary>
        /// Get current inventory for a client company part number
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="partnumber"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Itemsbybin>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("itemBinDistribution/{companyid}/{companyClient}/{partnumber}")]

        public async Task<IActionResult> GetItemBinsDistribution(int companyid, string companyClient, string partnumber, CancellationToken ct)
        {
           return await HandleApi(ct => _service.GetItemBinsDistribution(companyid, companyClient, partnumber,  ct), ct);

        }


        /// <summary>
        /// Get all transaction associated to Document Number (ASN, Pick Process, Repair ID)
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="DocumentNumber"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(WMSTransactionTm), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("all-transactions/{companyid}/{companyClient}/{DocumentNumber}")]
        public async Task<IActionResult> GetAllTransactionByDocumentNumber(int companyid, string companyClient, string DocumentNumber, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetAllTransactionByDocumentNumber(companyid, companyClient, DocumentNumber, ct), ct);


        }

        /// <summary>
        /// Get WMS Inventory transaction by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="transactionid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(WMSTransactionTm), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("inventory-transaction-by-id/{companyid}/{companyClient}/{transactionid}")]
        public async Task<IActionResult> GetInventoryTransactionById(int companyid, string companyClient, int transactionid, CancellationToken ct)
        {
            return await HandleApi(ct => _service.GetInventoryTransactionById(companyid, companyClient, transactionid, ct), ct);


        }

    }
}
