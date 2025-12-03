using DUNES.UI.Services.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DUNES.UI.Filters
{
    public class BreadcrumbFilter : IAsyncActionFilter
    {
        private const string SESSION_LEVEL_KEY = "LAST_LEVEL1";
        private readonly IMenuClientUIService _menuService;

        public BreadcrumbFilter(IMenuClientUIService menuService)
        {
            _menuService = menuService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var http = context.HttpContext;

            // Identificar controller/action actuales
            var controller = context.RouteData.Values["controller"]?.ToString();
            var action = context.RouteData.Values["action"]?.ToString();

            // En /Menu/Level el breadcrumb ya se arma y (opcional) se puede ACTUALIZAR el level en sesión
            var isMenuLevel = string.Equals(controller, "Menu", StringComparison.OrdinalIgnoreCase)
                           && string.Equals(action, "Level", StringComparison.OrdinalIgnoreCase);

            // Si no hay token, no armamos menú (evita llamadas innecesarias)
            var token = http.Session.GetString("JWToken");

            // —— Resolver el level1 (action args -> query -> session) ——
            string? code = null;

            // 1) De argumentos del action (si existe parámetro level1)
            if (context.ActionArguments.TryGetValue("level1", out var val) && val is string s1 && !string.IsNullOrWhiteSpace(s1))
                code = s1;

            // 2) De querystring (?level1=XXXX)
            if (string.IsNullOrWhiteSpace(code))
                code = http.Request.Query["level1"].FirstOrDefault();

            // 3) De sesión (último válido)
            if (string.IsNullOrWhiteSpace(code))
                code = http.Session.GetString(SESSION_LEVEL_KEY);

            // Si estamos en Menu/Level y llegó un code explícito, **persistir** en sesión
            if (isMenuLevel && !string.IsNullOrWhiteSpace(code))
                http.Session.SetString(SESSION_LEVEL_KEY, code);

            // Si estamos en otra ruta y llegó code (por args o query), **refrescar** sesión también
            if (!isMenuLevel && !string.IsNullOrWhiteSpace(code))
                http.Session.SetString(SESSION_LEVEL_KEY, code);

            // —— Construir breadcrumb solo si hay token + code ——
            if (!string.IsNullOrWhiteSpace(token) && !string.IsNullOrWhiteSpace(code))
            {
                var bc = await _menuService.GetBreadcrumbAsync(code!, token);
                if (bc != null && context.Controller is Controller c)
                    c.ViewData["Breadcrumb"] = bc;
                else if (context.Controller is Controller c2)
                    c2.ViewData["Breadcrumb"] = null; // limpiar si el code ya no es válido
            }

            await next();
        }
    }
}
