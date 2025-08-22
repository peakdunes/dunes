using DUNES.Shared.Models;
using DUNES.UI.Helpers; // Asegúrate de tener este using si usas MessageHelper
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
        /// <summary>
        /// Obtiene el token JWT almacenado en la sesión.
        /// </summary>
        /// <returns>El token como string o null si no existe.</returns>
        protected string? GetToken()
        {
            return HttpContext.Session.GetString("JWToken");
        }

        /// <summary>
        /// Redirige al usuario a la pantalla de login en caso de token inválido.
        /// También muestra un mensaje de error usando MessageHelper.
        /// </summary>
        /// <returns>Redirección al Index/Home.</returns>
        protected IActionResult RedirectToLogin()
        {
            MessageHelper.SetMessage(this, "danger", "Token Invalid. Please try again.", MessageDisplay.Inline);
            return RedirectToAction("Index", "Home");
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
    }
}
