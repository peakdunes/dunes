using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.Auth;
using DUNES.UI.Services.WMS.Masters.TransactionTypeClient;
using DUNES.UI.Services.WMS.Masters.TransactionTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DUNES.UI.Controllers.WMS.Masters.TransactionTypeClient
{
    [Authorize]
    public class TransactionTypeClientUIController : BaseController
    {
        private readonly ITransactionTypeClientWMSUIService _service;
        private readonly ITransactionTypesWMSUIService _transactionTypeService;
        private readonly IMenuClientUIService _menuClientService;

        private const string MENU_CODE_INDEX = "01020308";
        private const string MENU_CODE_CRUD = "01020308ZZ";

        private const string PERMISSION_ACCESS = "Masters.TransactionTypeClient.Access";
        private const string PERMISSION_CREATE = "Masters.TransactionTypeClient.Create";
        private const string PERMISSION_UPDATE = "Masters.TransactionTypeClient.Update";
        private const string PERMISSION_DELETE = "Masters.TransactionTypeClient.Delete";

        public TransactionTypeClientUIController(
            ITransactionTypeClientWMSUIService service,
            IMenuClientUIService menuClientService,
            ITransactionTypesWMSUIService transactionTypeService,
             IAuthPermissionUIService authPermissionUIService,
            IUserPermissionSessionHelper permissionSessionHelper)
            : base(permissionSessionHelper, authPermissionUIService)
        {
            _service = service;
            _transactionTypeService = transactionTypeService;
            _menuClientService = menuClientService;
        }

        [HttpGet]
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

            await LoadCrudActionsAsync("Masters", "TransactionTypeClient", ct);

            return await HandleAsync(async ct =>
            {
                var result = await _service.GetAllAsync(CurrentToken, ct);

                if (!result.Success || result.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    return View(new List<WMSTransactionTypeClientReadDTO>());
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
                new BreadcrumbItem { Text = "New Transaction Type Mapping", Url = null });

            await LoadTransactionTypesAsync(ct);

            return View(new WMSTransactionTypeClientCreateDTO
            {
                Active = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WMSTransactionTypeClientCreateDTO dto, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_CREATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            if (!ModelState.IsValid)
            {
                await LoadTransactionTypesAsync(ct);
                return View(dto);
            }

            return await HandleAsync(async ct =>
            {
                var result = await _service.CreateAsync(dto, CurrentToken, ct);

                if (!result.Success)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    await LoadTransactionTypesAsync(ct);
                    return View(dto);
                }

                MessageHelper.SetMessage(this, "success", result.Message, MessageDisplay.Inline);
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

            return await HandleAsync(async ct =>
            {
                var result = await _service.GetByIdAsync(id, CurrentToken, ct);

                if (!result.Success || result.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Toast);
                    return RedirectToAction(nameof(Index));
                }

                await SetMenuBreadcrumbAsync(
                    MENU_CODE_CRUD,
                    _menuClientService,
                    ct,
                    CurrentToken,
                    new BreadcrumbItem { Text = "Edit Transaction Type Mapping", Url = null });

                await LoadTransactionTypesAsync(ct);

                var dto = new WMSTransactionTypeClientUpdateDTO
                {
                    Id = result.Data.Id,
                    TransactionTypeId = result.Data.TransactionTypeId,
                    TransactionTypeName = result.Data.TransactionTypeName,
                    Active = result.Data.Active
                };

                return View(dto);
            }, ct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WMSTransactionTypeClientUpdateDTO dto, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_UPDATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            if (!ModelState.IsValid)
            {
                await LoadTransactionTypesAsync(ct);
                return View(dto);
            }

            return await HandleAsync(async ct =>
            {
                var result = await _service.UpdateAsync(id, dto, CurrentToken, ct);

                if (!result.Success)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    await LoadTransactionTypesAsync(ct);
                    return View(dto);
                }

                MessageHelper.SetMessage(this, "success", result.Message, MessageDisplay.Inline);
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

            return await HandleAsync(async ct =>
            {
                await SetMenuBreadcrumbAsync(
                    MENU_CODE_CRUD,
                    _menuClientService,
                    ct,
                    CurrentToken,
                    new BreadcrumbItem { Text = "Delete Transaction Type Mapping", Url = null });

                var result = await _service.GetByIdAsync(id, CurrentToken, ct);

                if (!result.Success || result.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Toast);
                    return RedirectToAction(nameof(Index));
                }

                return View(result.Data);
            }, ct);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, WMSTransactionTypeClientReadDTO dto, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_DELETE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {
                var result = await _service.DeleteAsync(id, CurrentToken, ct);

                MessageHelper.SetMessage(
                    this,
                    result.Success ? "success" : "danger",
                    result.Message,
                    MessageDisplay.Inline);

                return RedirectToAction(nameof(Index));
            }, ct);
        }

        private async Task LoadTransactionTypesAsync(CancellationToken ct)
        {
            var master = await _transactionTypeService.GetAllAsync(CurrentToken!, ct);

            var mapped = await _service.GetEnabledAsync(CurrentToken!, ct);
            var mappedIds = (mapped.Data ?? new List<WMSTransactionTypeClientReadDTO>())
                .Select(x => x.TransactionTypeId)
                .ToHashSet();

            var available = master.Data
                .Where(x => !mappedIds.Contains(x.Id) && x.Active)
                .ToList();

            ViewBag.TransactionTypes = new SelectList(available, "Id", "Name");
        }
    }
}