using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.WMS.Masters.TransactionConceptClient;
using DUNES.UI.Services.WMS.Masters.TransactionConcepts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DUNES.UI.Controllers.WMS.Masters.TransactionConceptClient
{
    [Authorize]
    public class TransactionConceptClientUIController : BaseController
    {
        private readonly ITransactionConceptClientWMSUIService _service;
        private readonly ITransactionConceptsWMSUIService _transactionConceptService;
        private readonly IMenuClientUIService _menuClientService;

        private const string MENU_CODE_INDEX = "01020307";
        private const string MENU_CODE_CRUD = "01020307ZZ";

        private const string PERMISSION_ACCESS = "Masters.TransactionConceptClient.Access";
        private const string PERMISSION_CREATE = "Masters.TransactionConceptClient.Create";
        private const string PERMISSION_UPDATE = "Masters.TransactionConceptClient.Update";
        private const string PERMISSION_DELETE = "Masters.TransactionConceptClient.Delete";

        public TransactionConceptClientUIController(
            ITransactionConceptClientWMSUIService service,
            IMenuClientUIService menuClientService,
            ITransactionConceptsWMSUIService transactionConceptService,
            IUserPermissionSessionHelper permissionSessionHelper)
            : base(permissionSessionHelper)
        {
            _service = service;
            _transactionConceptService = transactionConceptService;
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

            return await HandleAsync(async ct =>
            {
                var result = await _service.GetAllAsync(CurrentToken, ct);

                if (!result.Success || result.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    return View(new List<WMSTransactionConceptClientReadDTO>());
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
                new BreadcrumbItem { Text = "New Transaction Concept Mapping", Url = null });

            await LoadTransactionConceptsAsync(ct);

            return View(new WMSTransactionConceptClientCreateDTO
            {
                Active = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WMSTransactionConceptClientCreateDTO dto, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_CREATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            if (!ModelState.IsValid)
            {
                await LoadTransactionConceptsAsync(ct);
                return View(dto);
            }

            return await HandleAsync(async ct =>
            {
                var result = await _service.CreateAsync(dto, CurrentToken, ct);

                if (!result.Success)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    await LoadTransactionConceptsAsync(ct);
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
                    new BreadcrumbItem { Text = "Edit Transaction Concept Mapping", Url = null });

                await LoadTransactionConceptsAsync(ct);

                var dto = new WMSTransactionConceptClientUpdateDTO
                {
                    Id = result.Data.Id,
                    TransactionConceptId = result.Data.TransactionConceptId,
                    TransactionConceptName = result.Data.TransactionConceptName,
                    Active = result.Data.Active
                };

                return View(dto);
            }, ct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WMSTransactionConceptClientUpdateDTO dto, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_UPDATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            if (!ModelState.IsValid)
            {
                await LoadTransactionConceptsAsync(ct);
                return View(dto);
            }

            return await HandleAsync(async ct =>
            {
                var result = await _service.UpdateAsync(id, dto, CurrentToken, ct);

                if (!result.Success)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    await LoadTransactionConceptsAsync(ct);
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
                    new BreadcrumbItem { Text = "Delete Transaction Concept Mapping", Url = null });

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
        public async Task<IActionResult> DeleteConfirmed(int id, WMSTransactionConceptClientReadDTO dto, CancellationToken ct)
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

        private async Task LoadTransactionConceptsAsync(CancellationToken ct)
        {
            var master = await _transactionConceptService.GetAllAsync(CurrentToken!, ct);

            var mapped = await _service.GetEnabledAsync(CurrentToken!, ct);
            var mappedIds = (mapped.Data ?? new List<WMSTransactionConceptClientReadDTO>())
                .Select(x => x.TransactionConceptId)
                .ToHashSet();

            var available = master.Data
                .Where(x => !mappedIds.Contains(x.Id) && x.Active)
                .ToList();

            ViewBag.TransactionConcepts = new SelectList(available, "Id", "Name");
        }
    }
}