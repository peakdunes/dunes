using DUNES.API.Models.Inventory;
using DUNES.API.Services.Inventory.ASN.Queries;
using DUNES.API.Services.Inventory.Common.Queries;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.Controllers.Inventory.Common
{

    /// <summary>
    /// All inventory transactions queries
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommonQueryINVController : BaseController
    {


        private readonly ICommonQueryINVService _commonQueryINVService;

        /// <summary>
        /// injection dependency
        /// </summary>
        /// <param name="commonQueryINVService"></param>
        public CommonQueryINVController(ICommonQueryINVService commonQueryINVService)
        {

            _commonQueryINVService = commonQueryINVService;
        }

        /// <summary>
        /// get all inventory transactions for a document number
        /// </summary>
        /// <param name="DocumentNumber"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<List<TzebB2bReplacementPartsInventoryLogDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [HttpGet("inv-transactions-by-document/{DocumentNumber}")]
        public async Task<IActionResult> GetAllInventoryTransactionsByDocument(string DocumentNumber, CancellationToken ct)
        {
            return await HandleApi(async ct =>
            {

                return await _commonQueryINVService.GetAllInventoryTransactionsByDocument(DocumentNumber, ct);
            }, ct);
        }


        /// <summary>
        /// Get all inventory transactions for a Document Number and a search Start Date
        /// </summary>
        /// <param name="DocumentNumber"></param>
        /// <param name="ct"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<List<TzebB2bReplacementPartsInventoryLogDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [HttpGet("inv-transactions-by-document-date/{DocumentNumber}")]
        public async Task<IActionResult> GetAllInventoryTransactionsByDocumentStartDate(string DocumentNumber, [FromQuery] DateTime startDate, CancellationToken ct)
        {
            return await HandleApi(async ct =>
            {

                return await _commonQueryINVService.GetAllInventoryTransactionsByDocumentStartDate(DocumentNumber, startDate, ct);
            }, ct);
        }

       


        /// <summary>
        /// Get all inventory transactions for a Part Number ID
        /// </summary>
        /// <param name="PartNumberId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<List<TzebB2bReplacementPartsInventoryLogDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [HttpGet("inv-transactions-by-partnumberid/{PartNumberId}")]
        public async Task<IActionResult> GetAllInventoryTransactionsByPartNumberId(int PartNumberId, CancellationToken ct)
        {
            return await HandleApi(async ct =>
            {

                return await _commonQueryINVService.GetAllInventoryTransactionsByPartNumberId(PartNumberId, ct);
            }, ct);
        }

        /// <summary>
        /// Get all Division for a company
        /// </summary>
        /// <param name="CompanyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<TdivisionCompanyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [HttpGet("division-by-companyclient/{CompanyClient}")]
        public async Task<IActionResult> GetDivisionByCompanyClient(string CompanyClient, CancellationToken ct)
        {
            return await HandleApi(async ct =>
            {

                return await _commonQueryINVService.GetDivisionByCompanyClient(CompanyClient, ct);
            }, ct);
        }

        /// <summary>
        /// Get all input - output call for a ASN - Pick Process Document
        /// </summary>
        /// <param name="DocumentId"></param>
        /// <param name="processtype"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ApiResponse<PickProcessRequestDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("all-calls/{DocumentId}/{processtype}")]
        public async Task<IActionResult> GetAllCalls(string DocumentId, string processtype, CancellationToken ct)
        {
            return await HandleApi(async ct =>
            {
                return await _commonQueryINVService.GetAllCalls(DocumentId, processtype, ct);


            }, ct);
        }

    }
}
