using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.Auth;
using DUNES.UI.Services.WMS.Masters.TransactionConcepts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.UI.Controllers.WMS.Masters.TransactionConcepts
{
    [Authorize]
    public class TransactionConceptsUIController : BaseController
    {
        private readonly ITransactionConceptsWMSUIService _service;
        private readonly IMenuClientUIService _menuClientService;

        private const string MENU_CODE_INDEX = "01020808";
        private const string MENU_CODE_CRUD = "01020808ZZ";

        private const string PERMISSION_ACCESS = "Masters.TransactionConcepts.Access";
        private const string PERMISSION_CREATE = "Masters.TransactionConcepts.Create";
        private const string PERMISSION_UPDATE = "Masters.TransactionConcepts.Update";
        private const string PERMISSION_DELETE = "Masters.TransactionConcepts.Delete";

        public TransactionConceptsUIController(
            ITransactionConceptsWMSUIService service,
            IMenuClientUIService menuClientService,
             IAuthPermissionUIService authPermissionUIService,
            IUserPermissionSessionHelper permissionSessionHelper)
            : base(permissionSessionHelper, authPermissionUIService)
        {
            _service = service;
            _menuClientService = menuClientService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_ACCESS))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_INDEX,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "", Url = null });


            await LoadCrudActionsAsync("Masters", "TransactionConcepts", ct);

            return await HandleAsync(async ct =>
            {
                var result = await _service.GetAllAsync(CurrentToken, ct);

                if (!result.Success || result.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message ?? "Error loading transaction concepts.", MessageDisplay.Inline);
                    return View(new List<WMSTransactionconceptsReadDTO>());
                }

                return View(result.Data);
            }, ct);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_CREATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "New Transaction Concept", Url = null });

            return View(new WMSTransactionconceptsCreateDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WMSTransactionconceptsCreateDTO dto, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_CREATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {
                var result = await _service.CreateAsync(dto, CurrentToken!, ct);

                if (!result.Success)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message ?? "Error creating transaction concept.", MessageDisplay.Inline);
                    return View(dto);
                }

                MessageHelper.SetMessage(this, "success", result.Message ?? "Transaction Concept created.", MessageDisplay.Inline);
                return RedirectToAction(nameof(Index));
            }, ct);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_UPDATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "Edit Transaction Concept", Url = null });

            return await HandleAsync(async ct =>
            {
                var res = await _service.GetByIdAsync(id, CurrentToken!, ct);

                if (!res.Success || res.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", res.Message ?? "Transaction Concept not found.", MessageDisplay.Inline);
                    return RedirectToAction(nameof(Index));
                }

                var model = new WMSTransactionconceptsUpdateDTO
                {
                    Id = res.Data.Id,
                    Name = res.Data.Name,
                    Observations = res.Data.Observations,
                    Active = res.Data.Active
                };

                return View(model);
            }, ct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WMSTransactionconceptsUpdateDTO dto, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_UPDATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {
                var res = await _service.UpdateAsync(id, dto, CurrentToken!, ct);

                if (!res.Success)
                {
                    MessageHelper.SetMessage(this, "danger", res.Message ?? "Error updating transaction concept.", MessageDisplay.Inline);
                    return View(dto);
                }

                MessageHelper.SetMessage(this, "success", res.Message ?? "Transaction Concept updated.", MessageDisplay.Inline);
                return RedirectToAction(nameof(Index));
            }, ct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetActive(int id, bool isActive, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_UPDATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {
                var res = await _service.SetActiveAsync(id, isActive, CurrentToken!, ct);

                MessageHelper.SetMessage(
                    this,
                    res.Success ? "success" : "danger",
                    res.Message ?? (res.Success ? "Updated." : "Error updating."),
                    MessageDisplay.Inline);

                return RedirectToAction(nameof(Index));
            }, ct);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_DELETE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "Delete Transaction Concept", Url = null });

            return await HandleAsync(async ct =>
            {
                var res = await _service.GetByIdAsync(id, CurrentToken!, ct);

                if (!res.Success || res.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", res.Message ?? "Transaction Concept not found.", MessageDisplay.Inline);
                    return RedirectToAction(nameof(Index));
                }

                return View(res.Data);
            }, ct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, WMSTransactionconceptsReadDTO dto, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_DELETE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {
                var res = await _service.DeleteByIdAsync(CurrentToken!, id, ct);

                MessageHelper.SetMessage(
                    this,
                    res.Success ? "success" : "danger",
                    res.Message ?? (res.Success ? "Deleted." : "Error deleting."),
                    MessageDisplay.Inline);

                return RedirectToAction(nameof(Index));
            }, ct);
        }
    }
}