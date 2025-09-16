using DUNES.API.ServicesWMS.Inventory.Transactions;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.ControllersWMS.Inventory.Transactions
{

    /// <summary>
    /// All WMS inventory Transactions
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionWMSINVController : ControllerBase
    {


        private readonly ITransactionsWMSINVService _transactionService;

        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="transactionService"></param>
        public TransactionWMSINVController(ITransactionsWMSINVService transactionService)
        {

            _transactionService = transactionService;
            
        }

       
        /// <summary>
        /// Create a new WMS Inventory transaction
        /// </summary>
        /// <param name="objcreate"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<PickProcessRequestDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [HttpPost("wms-create-transaction/{objcreate}")]
        public async Task<IActionResult> CreateInventoryTransaction(NewInventoryTransactionTm objcreate)
        {
            var response = await _transactionService.CreateInventoryTransaction(objcreate);

            return StatusCode(response.StatusCode, response);
        }
    }
}
