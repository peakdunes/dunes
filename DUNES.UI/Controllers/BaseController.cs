using DUNES.Shared.Models;
using DUNES.UI.Helpers;

using DUNES.UI.Infrastructure;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace DUNES.UI.Controllers
{
    /// <summary>
    /// Controlador base para DUNES.UI que centraliza lógica común como:
    /// - Validación de sesión (token)
    /// - Manejo de errores globales
    /// - Soporte para CancellationToken (por cierre de navegador)
    /// - Manejo de mensajes al usuario (MessageHelper)
    /// </summary>
    public class BaseController : Controller
    {
        protected readonly IUserPermissionSessionHelper _permissionSessionHelper;

        public BaseController(IUserPermissionSessionHelper permissionSessionHelper)
        {
            _permissionSessionHelper = permissionSessionHelper;
        }

        /// <summary>
        /// Obtiene el token JWT almacenado en la sesión.
        /// </summary>
        /// <returns>El token como string o null si no existe.</returns>
        protected string? CurrentToken => HttpContext.GetUserSession()?.Token;

        /// <summary>
        /// Gets the current CompanyId from the session.
        /// </summary>
        protected int CurrentCompanyId => HttpContext.GetUserSession()?.CompanyId ?? 0;

        /// <summary>
        /// Gets the current CompanyClientId from the session.
        /// </summary>
        protected int CurrentCompanyClientId => HttpContext.GetUserSession()?.CompanyClientId ?? 0;

        /// <summary>
        /// Gets the current CompanyClientId from the session.
        /// </summary>
        protected int CurrentcompaniesContractId => HttpContext.GetUserSession()?.companiesContractId ?? 0;

        /// <summary>
        /// Returns Forbid when the current user does not have the specified permission.
        /// Returns null when access is allowed.
        /// </summary>
        /// <param name="permission">Permission key.</param>
        /// <returns>Forbid result or null.</returns>
        protected IActionResult? RequirePermission(string permission)
        {
            if (!_permissionSessionHelper.HasPermission(permission))
                return Forbid();

            return null;
        }

        /// <summary>
        /// Returns RedirectToLogin when token is missing, Forbid when permission is missing,
        /// or null when access is allowed.
        /// </summary>
        /// <param name="permission">Permission key.</param>
        /// <returns>Redirect/Forbid result or null.</returns>
        protected IActionResult? RequireTokenAndPermission(string permission)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

            if (!_permissionSessionHelper.HasPermission(permission))
                return Forbid();

            return null;
        }

        /// <summary>
        /// Determines whether the current user has the specified permission.
        /// </summary>
        /// <param name="permission">Permission key.</param>
        /// <returns>True when the user has the permission.</returns>
        protected bool HasPermission(string permission)
        {
            return _permissionSessionHelper.HasPermission(permission);
        }

        /// <summary>
        /// Redirige al usuario a la pantalla de login en caso de token inválido.
        /// También muestra un mensaje de error usando MessageHelper.
        /// </summary>
        /// <returns>Redirección al Index/Home.</returns>
        protected IActionResult RedirectToLogin()
        {
            MessageHelper.SetMessage(this, "danger", "Your session has expired. Please sign in again. Please try again.", MessageDisplay.Inline);
            //return RedirectToAction("Index", "Home");
            return RedirectToAction("Login", "Auth");
        }

        /// <summary>
        /// Ejecuta una acción asincrónica manejando cancelación y errores de forma centralizada.
        /// Se recomienda usar en todos los métodos async que consumen servicios externos.
        /// esta notacion "Func<CancellationToken, Task<IActionResult>> action, CancellationToken ct"
        /// significa que yo le envio a una funcion un CancellationToken y el me retorna un Task<IActionResult>
        /// y el parametro del CancellationToken es el ct
        /// </summary>
        /// <param name="action">Función a ejecutar que recibe un CancellationToken.</param>
        /// <param name="ct">Token de cancelación propagado automáticamente por el framework.</param>
        /// <returns>Un IActionResult con la vista o redirección correspondiente.</returns>
        protected async Task<IActionResult> HandleAsync(Func<CancellationToken, Task<IActionResult>> action, CancellationToken ct)
        {
            if (ct.IsCancellationRequested)
                return new StatusCodeResult(StatusCodes.Status499ClientClosedRequest); // cliente canceló

            try
            {
                return await action(ct);
            }
            catch (OperationCanceledException)
            {
                return new StatusCodeResult(StatusCodes.Status499ClientClosedRequest);
            }
            catch
            {
                throw;
            }
        }

        protected void SetBreadcrumb(params BreadcrumbItem[] items)
        {
            ViewData["Breadcrumb"] = items.ToList();
        }

        /// <summary>
        /// Construye la miga de pan tomando como base un código de menú (01, 0101, etc.)
        /// y permite agregar items extra al final (por ejemplo Countries, Create, Edit).
        /// </summary>
        protected async Task SetMenuBreadcrumbAsync(
            string menuCode,
            IMenuClientUIService menuClientService,
            CancellationToken ct,
            string token,
            params BreadcrumbItem[] extraItems)
        {
            var isCrud = menuCode.Contains("ZZ");   // tu marca para Create/Edit/etc.
            var newCode = menuCode.Trim();

            if (isCrud)
            {
                // protección por si alguien pasa un código muy corto por error
                if (menuCode.Length >= 4)
                {
                    newCode = menuCode.Substring(0, menuCode.Length - 2);
                }
            }

            var breadcrumb = await menuClientService.GetBreadcrumbAsync(token, newCode, ct);

            // Corregimos Home para que apunte bien
            if (breadcrumb.Count > 0)
                breadcrumb[0].Url = Url.Action("Index", "Home");

            if (isCrud && breadcrumb.Count > 0)
            {
                // quitamos el último del menú (normalmente el listado)
                breadcrumb.RemoveAt(breadcrumb.Count - 1);
            }

            if (extraItems != null && extraItems.Length > 0)
            {
                breadcrumb.AddRange(extraItems);
            }

            SetBreadcrumb(breadcrumb.ToArray());
        }
    }
}