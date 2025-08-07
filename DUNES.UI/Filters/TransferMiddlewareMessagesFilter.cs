using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace DUNES.UI.Filters
{
    /// <summary>
    /// Filtro global que transfiere automáticamente mensajes desde HttpContext.Items hacia TempData,
    /// permitiendo que un middleware comunique mensajes a las vistas Razor (ej: toasts tras redirección).
    /// </summary>
    public class TransferMiddlewareMessagesFilter : IActionFilter
    {
        private readonly ILogger<TransferMiddlewareMessagesFilter> _logger;

        public TransferMiddlewareMessagesFilter(ILogger<TransferMiddlewareMessagesFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Filtro ejecutado en: {Action}", context.ActionDescriptor.DisplayName);

            var controller = context.Controller as Controller;
            var httpContext = context.HttpContext;

            if (controller != null && httpContext.Items.ContainsKey("ApiMessage"))
            {
                controller.TempData["ApiMessage"] = httpContext.Items["ApiMessage"];
                controller.TempData["ApiType"] = httpContext.Items["ApiType"];

                _logger.LogInformation("Mensaje transferido a TempData → {Message} ({Type})",
                    controller.TempData["ApiMessage"], controller.TempData["ApiType"]);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No se necesita lógica posterior aquí.
        }
    }
}