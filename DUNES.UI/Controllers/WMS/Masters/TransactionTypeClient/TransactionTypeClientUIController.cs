using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.WMS.Masters.TransactionTypeClient;
using DUNES.UI.Services.WMS.Masters.TransactionTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DUNES.UI.Controllers.WMS.Masters.TransactionTypeClient
{
    /// <summary>
    /// UI controller for client-specific Transaction Type configuration.
    /// </summary>
    public class TransactionTypeClientUIController : BaseController
    {
        private readonly ITransactionTypeClientWMSUIService _service;
        private readonly ITransactionTypesWMSUIService _transactionTypeService;
        private readonly IMenuClientUIService _menuClientService;

        private const string MENU_CODE_INDEX = "01020308";
        private const string MENU_CODE_CRUD = "01020308ZZ";

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionTypeClientUIController"/> class.
        /// </summary>
        /// <param name="service">UI service for client transaction type mappings.</param>
        /// <param name="menuClientService">Menu/breadcrumb service.</param>
        /// <param name="transactionTypeService">UI service for master transaction types.</param>
        public TransactionTypeClientUIController(
            ITransactionTypeClientWMSUIService service,
            IMenuClientUIService menuClientService,
            ITransactionTypesWMSUIService transactionTypeService)
        {
            _service = service;
            _transactionTypeService = transactionTypeService;
            _menuClientService = menuClientService;
        }

        /// <summary>
        /// Displays the list of transaction type mappings for the current client.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Index view with the current mappings.</returns>
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
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
                    return View(new List<WMSTransactionTypeClientReadDTO>());
                }

                return View(result.Data);
            }, ct);
        }

        /// <summary>
        /// Displays the create view for a new client transaction type mapping.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Create view.</returns>
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
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

        /// <summary>
        /// Creates a new client transaction type mapping.
        /// </summary>
        /// <param name="dto">Create DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Redirects to Index on success; otherwise redisplays the form.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            WMSTransactionTypeClientCreateDTO dto,
            CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "New Transaction Type Mapping", Url = null });

            if (!ModelState.IsValid)
            {
                await LoadTransactionTypesAsync(ct);
                return View(dto);
            }

            var result = await _service.CreateAsync(dto, CurrentToken, ct);

            if (!result.Success)
            {
                MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                await LoadTransactionTypesAsync(ct);
                return View(dto);
            }

            MessageHelper.SetMessage(this, "success", result.Message, MessageDisplay.Inline);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Displays the edit view for an existing client transaction type mapping.
        /// </summary>
        /// <param name="id">Mapping Id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Edit view.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

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
        }

        /// <summary>
        /// Updates an existing client transaction type mapping.
        /// </summary>
        /// <param name="id">Mapping Id.</param>
        /// <param name="dto">Update DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Redirects to Index on success; otherwise redisplays the form.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            WMSTransactionTypeClientUpdateDTO dto,
            CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

            if (!ModelState.IsValid)
            {
                await LoadTransactionTypesAsync(ct);
                return View(dto);
            }

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "Edit Transaction Type Mapping", Url = null });

            var result = await _service.UpdateAsync(id, dto, CurrentToken, ct);

            if (!result.Success)
            {
                MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                await LoadTransactionTypesAsync(ct);
                return View(dto);
            }

            MessageHelper.SetMessage(this, "success", result.Message, MessageDisplay.Inline);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Displays the delete confirmation view for a client transaction type mapping.
        /// </summary>
        /// <param name="id">Mapping Id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Delete confirmation view.</returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

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
        }

        /// <summary>
        /// Confirms and executes the deletion of a client transaction type mapping.
        /// </summary>
        /// <param name="id">Mapping Id.</param>
        /// <param name="dto">Read DTO used by the confirmation view.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Redirects to Index after delete attempt.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, WMSTransactionTypeClientReadDTO dto, CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

            var result = await _service.DeleteAsync(id, CurrentToken, ct);

            if (!result.Success)
            {
                MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                return RedirectToAction(nameof(Index));
            }

            MessageHelper.SetMessage(this, "success", result.Message, MessageDisplay.Inline);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Loads the Transaction Type master list into ViewBag for dropdown rendering.
        /// Only active and not-already-mapped items are included.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
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