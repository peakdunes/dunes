using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.WMS.Masters.TransactionConceptClient;
using DUNES.UI.Services.WMS.Masters.TransactionConcepts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Common;

namespace DUNES.UI.Controllers.WMS.Masters.TransactionConceptClient
{
    /// <summary>
    /// UI controller for client-specific Transaction Concept configuration.
    /// </summary>
    public class TransactionConceptClientUIController : BaseController
    {
        private readonly ITransactionConceptClientWMSUIService _service;
        private readonly ITransactionConceptsWMSUIService _transactionConceptService;
        private readonly IMenuClientUIService _menuClientService;

        private const string MENU_CODE_INDEX = "01020307";
        private const string MENU_CODE_CRUD = "01020307ZZ";

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionConceptClientUIController"/> class.
        /// </summary>
        /// <param name="service">UI service for client transaction concept mappings.</param>
        /// <param name="transactionConceptService">UI service for master transaction concepts.</param>
        public TransactionConceptClientUIController(
            ITransactionConceptClientWMSUIService service,
             IMenuClientUIService menuClientService,
            ITransactionConceptsWMSUIService transactionConceptService)
        {
            _service = service;
            _transactionConceptService = transactionConceptService;
            _menuClientService = menuClientService;
        }

        /// <summary>
        /// Displays the list of transaction concept mappings for the current client.
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
                    return View(new List<WMSTransactionConceptClientReadDTO>());
                }

                return View(result.Data);
            }, ct);
        }

        /// <summary>
        /// Displays the create view for a new client transaction concept mapping.
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
                new BreadcrumbItem { Text = "New Transaction Concept Mapping", Url = null });

            await LoadTransactionConceptsAsync(ct);

            return View(new WMSTransactionConceptClientCreateDTO
            {
                Active = true
            });
        }

        /// <summary>
        /// Creates a new client transaction concept mapping.
        /// </summary>
        /// <param name="dto">Create DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Redirects to Index on success; otherwise redisplays the form.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            WMSTransactionConceptClientCreateDTO dto,
            CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();


            await SetMenuBreadcrumbAsync(
               MENU_CODE_CRUD,
               _menuClientService,
               ct,
               CurrentToken,
               new BreadcrumbItem { Text = "New Transaction Concept Mapping", Url = null });

            if (!ModelState.IsValid)
            {
                await LoadTransactionConceptsAsync(ct);
                return View(dto);
            }

            var result = await _service.CreateAsync(dto, CurrentToken, ct);

            if (!result.Success)
            {
                MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                await LoadTransactionConceptsAsync(ct);
                return View(dto);
            }
            // MessageHelper.SetMessage(this, "success", result.Message, MessageDisplay.Toast);
            MessageHelper.SetMessage(this, "success", result.Message, MessageDisplay.Inline);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Displays the edit view for an existing client transaction concept mapping.
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
        }

        /// <summary>
        /// Updates an existing client transaction concept mapping.
        /// </summary>
        /// <param name="id">Mapping Id.</param>
        /// <param name="dto">Update DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Redirects to Index on success; otherwise redisplays the form.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            WMSTransactionConceptClientUpdateDTO dto,
            CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

            if (!ModelState.IsValid)
            {
                await LoadTransactionConceptsAsync(ct);
                return View(dto);
            }

            await SetMenuBreadcrumbAsync(
             MENU_CODE_CRUD,
             _menuClientService,
             ct,
             CurrentToken,
             new BreadcrumbItem { Text = "Edit Transaction Concept Mapping", Url = null });


            var result = await _service.UpdateAsync(id, dto, CurrentToken, ct);

            if (!result.Success)
            {
                MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                await LoadTransactionConceptsAsync(ct);
                return View(dto);
            }

            MessageHelper.SetMessage(this, "success", result.Message, MessageDisplay.Inline);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Displays the delete confirmation view for a client transaction concept mapping.
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
           new BreadcrumbItem { Text = "Delete Transaction Concept Mapping", Url = null });

            var result = await _service.GetByIdAsync(id, CurrentToken, ct);

            if (!result.Success || result.Data is null)
            {
                MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Toast);
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        /// <summary>
        /// Confirms and executes the deletion of a client transaction concept mapping.
        /// </summary>
        /// <param name="id">Mapping Id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Redirects to Index after delete attempt.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, WMSTransactionConceptClientReadDTO dto, CancellationToken ct)
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
        /// Loads the Transaction Concept master list into ViewBag for dropdown rendering.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        private async Task LoadTransactionConceptsAsync(CancellationToken ct)
        {
            var master = await _transactionConceptService.GetAllAsync(CurrentToken!, ct);

            // 2) Existing enabled mappings (enough to prevent duplicates)
            var mapped = await _service.GetEnabledAsync(CurrentToken!, ct);
            var mappedIds = (mapped.Data ?? new List<WMSTransactionConceptClientReadDTO>())
                .Select(x => x.TransactionConceptId)
                .ToHashSet();

            // 3) Available to enable
            var available = master.Data
                .Where(x => !mappedIds.Contains(x.Id) && x.Active)
                .ToList();

            ViewBag.TransactionConcepts = new SelectList(available, "Id", "Name");



        }
    }
}
