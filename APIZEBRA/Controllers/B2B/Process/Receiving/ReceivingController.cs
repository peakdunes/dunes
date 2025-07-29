using APIZEBRA.DTOs.B2B;
using APIZEBRA.Services.B2B.Common.Queries;
using APIZEBRA.Utils.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIZEBRA.Controllers.B2B.Process.Receiving
{  
    /// <summary>
    /// Receiving process for one repair numbre
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ReceivingController : ControllerBase
    {

        readonly ICommonQueryService _commonQueryService;

        /// <summary>
        /// new instance
        /// </summary>
        /// <param name="commonQueryService"></param>
        public ReceivingController(ICommonQueryService commonQueryService)
        {
            _commonQueryService = commonQueryService;

        }
       

    }
}
