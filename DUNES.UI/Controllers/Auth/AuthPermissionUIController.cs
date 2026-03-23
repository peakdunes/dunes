using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.UI.Controllers.Auth
{
    /// <summary>
    /// MVC controller for permission catalog management.
    /// </summary>
    public class AuthPermissionUIController : BaseController
    {
        private readonly IAuthPermissionUIService _authPermissionService;
        private readonly IMenuClientUIService _menuClientService;

        private const string MENU_CODE_INDEX = "0302";
        private const string MENU_CODE_CRUD = "0302ZZ";

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthPermissionUIController"/> class.
        /// </summary>
        /// <param name="authPermissionService">Permission UI service.</param>
        /// <param name="menuClientService">Menu client UI service.</param>
        public AuthPermissionUIController(
            IAuthPermissionUIService authPermissionService,
            IMenuClientUIService menuClientService)
        {
            _authPermissionService = authPermissionService;
            _menuClientService = menuClientService;
        }

        /// <summary>
        /// Displays the permission catalog list.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Permissions index view.</returns>
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
                var result = await _authPermissionService.GetAllAsync(CurrentToken, ct);

                if (!result.Success)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    return View(new List<AuthPermissionReadDTO>());
                }

                return View(result.Data ?? new List<AuthPermissionReadDTO>());
            }, ct);
        }

        /// <summary>
        /// Displays the create permission view.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Create permission view.</returns>
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
                new BreadcrumbItem { Text = "Add New Permission", Url = null });

            return await HandleAsync(async ct =>
            {
                var model = new AuthPermissionCreateDTO
                {
                    IsActive = true,
                    DisplayOrder = 1
                };

                return View(model);
            }, ct);
        }

        /// <summary>
        /// Creates a new permission in the catalog.
        /// </summary>
        /// <param name="model">Permission creation model.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Redirects to index on success, otherwise returns the create view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthPermissionCreateDTO model, CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "Permissions", Url = Url.Action(nameof(Index)) },
                new BreadcrumbItem { Text = "Create", Url = null });

            return await HandleAsync(async ct =>
            {
                NormalizeCreateModel(model);

                if (!ModelState.IsValid)
                {
                    MessageHelper.SetMessage(this, "danger", "Please review the required fields.", MessageDisplay.Inline);
                    return View(model);
                }

                var result = await _authPermissionService.CreateAsync(CurrentToken, model, ct);

                if (!result.Success)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    return View(model);
                }

                MessageHelper.SetMessage(this, "success", "Permission created successfully.", MessageDisplay.Inline);
                return RedirectToAction(nameof(Index));
            }, ct);
        }

        /// <summary>
        /// Normalizes text fields before sending the model to the API.
        /// </summary>
        /// <param name="model">Permission creation model.</param>
        private static void NormalizeCreateModel(AuthPermissionCreateDTO model)
        {
            model.PermissionKey = model.PermissionKey?.Trim() ?? string.Empty;
            model.GroupName = model.GroupName?.Trim() ?? string.Empty;
            model.ModuleName = model.ModuleName?.Trim() ?? string.Empty;
            model.ActionName = model.ActionName?.Trim() ?? string.Empty;
            model.Description = string.IsNullOrWhiteSpace(model.Description)
                ? null
                : model.Description.Trim();

            if (model.DisplayOrder < 1)
                model.DisplayOrder = 1;
        }
    }
}