using DUNES.API.DTOs.B2B;
using DUNES.API.Services.B2B.Common.Queries;
using DUNES.API.Utils.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.Controllers.B2B.Process.Receiving
{  
    /// <summary>
    /// Receiving process for one repair numbre
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ReceivingController : ControllerBase
    {

        readonly ICommonQueryB2BService _commonQueryService;

        /// <summary>
        /// new instance
        /// </summary>
        /// <param name="commonQueryService"></param>
        public ReceivingController(ICommonQueryB2BService commonQueryService)
        {
            _commonQueryService = commonQueryService;

        }
       

    }
}
