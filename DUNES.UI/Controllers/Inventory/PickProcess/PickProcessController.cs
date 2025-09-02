using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Services.Inventory.ASN;
using DUNES.UI.Services.Inventory.PickProcess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace DUNES.UI.Controllers.Inventory.PickProcess
{
                 
    public class PickProcessController : BaseController
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IPickProcessService _service;
        public readonly IConfiguration _config;
        public readonly int _companyDefault;


        public PickProcessController(IHttpClientFactory httpClientFactory, IConfiguration config, IPickProcessService service)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _service = service;
            _companyDefault = _config.GetValue<int>("companyDefault", 1);

        }


        public IActionResult Index()
        {
            return View();
        }
                             
        public async Task<IActionResult> PickProcessReceiving(string? pickprocessnumber, CancellationToken ct)
        {

            return await HandleAsync(async ct =>
            {
                PickProcessHdr objhdr = new PickProcessHdr();
                List<PickProcessItemDetail> objlist = new List<PickProcessItemDetail>();

                PickProcessDto objresult = new PickProcessDto
                {
                    PickProcessHdr = objhdr,
                    ListItems = objlist

                };


                if (string.IsNullOrWhiteSpace(pickprocessnumber))
                    return View(objresult);

                var token = GetToken();
                if (token == null)
                    return RedirectToLogin();

                var infopickProcess = await _service.GetPickProcessAllInfo(pickprocessnumber, token, ct);

                if (infopickProcess.Error != null)
                {
                    MessageHelper.SetMessage(this, "danger", infopickProcess.Error, MessageDisplay.Inline);
                    return View(objresult);
                }




                return View(objresult);

            }, ct);




        }
    }
}
