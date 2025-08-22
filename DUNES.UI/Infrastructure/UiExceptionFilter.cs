// DUNES.UI/Infrastructure/UiExceptionFilter.cs
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;

namespace DUNES.UI.Infrastructure
{
    /// <summary>
    /// Captura excepciones no manejadas en MVC, loggea y deja mensaje en TempData
    /// para que el layout muestre el toast con ApiMessage/ApiType.
    /// </summary>
    public sealed class UiExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<UiExceptionFilter> _logger;
        private readonly ITempDataDictionaryFactory _tempDataFactory;

        public UiExceptionFilter(
            ILogger<UiExceptionFilter> logger,
            ITempDataDictionaryFactory tempDataFactory)
        {
            _logger = logger;
            _tempDataFactory = tempDataFactory;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            var ex = context.Exception;
            var http = context.HttpContext;

            // 1) Log
            _logger.LogError(ex,
                "Unhandled exception at {Method} {Path} (User: {User})",
                http.Request?.Method,
                http.Request?.Path.Value,
                http.User?.Identity?.Name ?? "anon");

            // 2) Mensaje y tipo para el toast
            var message = ToUserMessage(ex, out var apiType); // apiType: success|info|warning|danger

            // 3) Guardar en TempData para el layout
            var tempData = _tempDataFactory.GetTempData(http);
            tempData["ApiType"] = apiType;
            tempData["ApiMessage"] = message;

            // 4) Marcar manejada
            context.ExceptionHandled = true;

            // 5) Redirección suave
            if (HttpMethods.IsGet(http.Request.Method))
            {
                var sameUrl = http.Request.Path + http.Request.QueryString;
                context.Result = new RedirectResult(sameUrl);
            }
            else
            {
                // PRG: evita re-post. Cambia "Home/Index" si prefieres otro destino.
                context.Result = new RedirectToActionResult("Index", "Home", routeValues: null);
            }

            return Task.CompletedTask;
        }

        private static string ToUserMessage(Exception ex, out string apiType)
        {
            // Mapear a tipos de tu toast: success | info | warning | danger
            apiType = "danger";

            return ex switch
            {
               

                HttpRequestException =>
                    "No hay conexión con el servicio. Verifica tu red o intenta más tarde.",

                TaskCanceledException or OperationCanceledException =>
                    "La solicitud tardó demasiado y fue cancelada. Intenta nuevamente.",

                UnauthorizedAccessException =>
                    "Tu sesión expiró. Inicia sesión nuevamente.",

                _ =>
                    "Ocurrió un error inesperado. Hemos registrado el incidente."
            };
        }
    }
}
