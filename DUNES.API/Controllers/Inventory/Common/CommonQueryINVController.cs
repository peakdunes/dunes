using DUNES.API.Services.Inventory.Common.Queries;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.Controllers.Inventory.Common
{

    /// <summary>
    /// Common Inventory queries (ASN - PickProcess
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CommonQueryINVController : ControllerBase
    {

        private readonly ICommonQueryINVService _service;

        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="service"></param>
        public CommonQueryINVController(ICommonQueryINVService service)
        {
            _service = service;
        }

        /// <summary>
        /// get all ASN information
        /// </summary>
        /// <param name="ShipmentNum"></param>
        /// <remarks>Returns header plus one or more detail lines.</remarks>
        /// <response code="200">Successful; returns data.</response>
        /// <response code="404">Not found if the delivery does not exist.</response>
        /// <returns>
        /// An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse{ASNDto}"/> 
        /// with the pick order header and details.
        /// </returns>
        [ProducesResponseType(typeof(ApiResponse<ASNDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]

        [HttpGet("asn-info/{ShipmentNum}")]
        public async Task<IActionResult> GetASNAllInfo (string ShipmentNum)
        {
            var response = await _service.GetASNAllInfo(ShipmentNum);

            return StatusCode(response.StatusCode, response);


        }

        /// <summary>Get all pick process information.</summary>
        /// <param name="DeliveryId">Delivery identifier to filter the pick process.</param>
        /// <remarks>Returns header plus one or more detail lines.</remarks>
        /// <response code="200">Successful; returns data.</response>
        /// <response code="404">Not found if the delivery does not exist.</response>
        /// <returns>
        /// An <see cref="ActionResult{T}"/> containing an <see cref="ApiResponse{PickOrderDto}"/> 
        /// with the pick order header and details.
        /// </returns>
        [ProducesResponseType(typeof(ApiResponse<PickProcessDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [HttpGet("pickprocess-info/{DeliveryId}")]
        public async Task<IActionResult> GetPickProcessAllInfo(string DeliveryId)
        {
            var response = await _service.GetPickProcessAllInfo(DeliveryId);

            return StatusCode(response.StatusCode, response);


        }

        
    }
}
