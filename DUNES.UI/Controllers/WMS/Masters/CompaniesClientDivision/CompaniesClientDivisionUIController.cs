using DUNES.Shared.DTOs.Masters;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.WMS.Masters.ClientCompanies;
using DUNES.UI.Services.WMS.Masters.CompaniesClientDivision;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DUNES.UI.Controllers.WMS.Masters.CompaniesClientDivision
{
    public class CompaniesClientDivisionUIController : BaseController
    {

        private readonly IClientCompaniesWMSUIService _companyClientService;
        private readonly IMenuClientUIService _menuClientService;
        private readonly ICompaniesClientDivisionWMSUIService _service;
        public readonly IConfiguration _config;
        public readonly int _companyDefault;
        private const string MENU_CODE_INDEX = "01020302";
        private const string MENU_CODE_CRUD = "01020302ZZ";


        public CompaniesClientDivisionUIController(ICompaniesClientDivisionWMSUIService service, IConfiguration config,
            IMenuClientUIService menuClientService, IClientCompaniesWMSUIService companyClientService)
        {
            _service = service;
            _config = config;
            _companyDefault = _config.GetValue("companyDefault", 1);
            _menuClientService = menuClientService;
            _companyClientService = companyClientService;
        }

        // GET: CompaniesClientDivisionUIController
        public async Task<IActionResult> Index(int? companyclientid, CancellationToken ct)
        {
            var token = GetToken();

            if (token == null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_INDEX,
               _menuClientService, ct, token,
               new BreadcrumbItem
               {
                   Text = "",   // actual
                   Url = null
               });

            if (companyclientid == null)
            {
                companyclientid = 0;
            }

            return await HandleAsync(async ct =>
            {
                List<WMSClientCompaniesReadDTO> listcompaniesactives = new List<WMSClientCompaniesReadDTO>();


                await LoadInfoAsync(token, ct, Convert.ToInt32(companyclientid));

                var listsdivision = await _service.GetAllCompaniesClientDivisionInformationByCompanyClient(Convert.ToInt32(companyclientid), token, ct);

                if (!listsdivision.Success)
                {
                    MessageHelper.SetMessage(this, "danger", listsdivision.Error!.ToString(), MessageDisplay.Inline);


                }

                if (companyclientid != 0)
                {
                    if (listsdivision.Data == null)
                    {
                        MessageHelper.SetMessage(this, "information", "Division not found for this company client", MessageDisplay.Inline);


                    }
                }

                return View(listsdivision.Data ?? new List<WMSCompanyClientDivisionReadDTO>());

            }, ct);
        }

        // GET: CompaniesClientDivisionUIController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CompaniesClientDivisionUIController/Create
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var token = GetToken();

            if (token == null)
                return RedirectToLogin();

            await LoadInfoAsync(token, ct, 0);

            await SetMenuBreadcrumbAsync(MENU_CODE_CRUD, _menuClientService, ct, token,
             new BreadcrumbItem
             {
                 Text = "New Company Client Division",
                 Url = null
             });





            return View();
        }

        // POST: CompaniesClientDivisionUIController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WMSCompanyClientDivisionDTO dto, CancellationToken ct)
        {
            var token = GetToken();

            if (token == null)
                return RedirectToLogin();


            await SetMenuBreadcrumbAsync(MENU_CODE_CRUD, _menuClientService, ct, token,
            new BreadcrumbItem
            {
                Text = "New Company Client Division",
                Url = null
            });

            return await HandleAsync(async ct =>
            {
                var infocompany = await _companyClientService.GetClientCompanyInformationByIdAsync(dto.Idcompanyclient!, token, ct);

                if (infocompany.Data == null)
                {
                    MessageHelper.SetMessage(this, "danger", "Company client not found", MessageDisplay.Inline);

                    await LoadInfoAsync(token, ct, dto.Idcompanyclient);

                    return View(dto);
                }

                var infodivisoname = await _service.GetCompanyClientDivisionByNameAsync(dto.Idcompanyclient!, dto.DivisionName!, token, ct);

                if (infodivisoname.Data != null)
                {
                    MessageHelper.SetMessage(this, "danger", "Company Client division name already exist", MessageDisplay.Inline);

                    await LoadInfoAsync(token, ct, dto.Idcompanyclient);

                    return View(dto);
                }

                var createrecord = await _service.AddClientCompanyDivisionAsync(dto, token, ct);

                if (!createrecord.Data)
                {
                    MessageHelper.SetMessage(this, "danger", $"Error creating this Company Client Division Error:{createrecord.Message}", MessageDisplay.Inline);


                    return View(dto);
                }

                MessageHelper.SetMessage(this, "success", $"Company Client Division created", MessageDisplay.Inline);


                return RedirectToAction(nameof(Index));
            }, ct);


        }

        // GET: CompaniesClientDivisionUIController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CompaniesClientDivisionUIController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompaniesClientDivisionUIController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CompaniesClientDivisionUIController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private async Task LoadInfoAsync(string token, CancellationToken ct, int idselected)
        {


            List<WMSClientCompaniesReadDTO> listcompaniesactives = new List<WMSClientCompaniesReadDTO>();

            var listcompanies = await _companyClientService.GetAllClientCompaniesInformation(token, ct);

            if (listcompanies.Data != null)
            {
                listcompaniesactives = listcompanies.Data.Where(x => x.Active == true).ToList();
            }
            if (idselected != 0)
            {

                ViewData["companiesclient"] = new SelectList(listcompaniesactives, "Id", "Name");
            }
            else
            {
                ViewData["companiesclient"] = new SelectList(listcompaniesactives, "Id", "Name", idselected);
            }
        }
    }
}

