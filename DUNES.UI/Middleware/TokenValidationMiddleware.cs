using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IdentityModel.Tokens.Jwt;

namespace DUNES.UI.Middleware
{
    public class TokenValidationMiddleware
    {

        /// <summary>
        /// indica que si la validacion es correcta
        /// continue al siguiente middleware
        /// </summary>
        private readonly RequestDelegate _next;
        private readonly ITempDataDictionaryFactory _tempDataFactory;
        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="next"></param>
        public TokenValidationMiddleware(RequestDelegate next, ITempDataDictionaryFactory tempDataFactory)
        {
            _next = next;
            _tempDataFactory = tempDataFactory;
        }


        /// <summary>
        /// Este metodo se ejecuta cada vez que llega una peticion Http
        /// recibe el objeto httpcontext con toda la informacion
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        //public async Task Invoke(HttpContext context)
        //{


        //    var path = context.Request.Path;


        //    // Rutas excluidas: no validan token porque en el caso de Auth no se
        //    //debe validar el token porque lo va ha solicitar, 
        //    var excludedPaths = new[]
        //    {
        //    "/Auth", "/Home", "/Password", "/css", "/js", "/images", "/lib", "/favicon.ico"
        //    };

        //    bool isExcluded = excludedPaths.Any(p => path.StartsWithSegments(p));
        //    if (isExcluded)
        //    {
        //        await _next(context); // Pasa al siguiente middleware sin  validar token
        //        return;
        //    }

        //    // Aquí validamos el token de sesión
        //    var token = context.Session.GetString("JWToken");



        //    if (!IsValidToken(token))
        //    {
        //        context.Session.Remove("JWToken");

        //        context.Items["ApiMessage"] = "Your token has expired.";
        //        context.Items["ApiType"] = "danger";

        //        // Redirigir a login o mostrar error
        //        context.Response.Redirect("/Auth/Login");
        //        return;
        //    }

        //    await _next(context); // Continuar si todo está bien
        //}

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLowerInvariant() ?? "";

            // ✅ Solo rutas públicas (login + estáticos + error)
            var excluded = new[]
            {
        "/auth/login",
        "/password",
        "/css",
        "/js",
        "/images",
        "/lib",
        "/favicon.ico",
        "/error"
    };

            if (excluded.Any(p => path.StartsWith(p)))
            {
                await _next(context);
                return;
            }

            var token = context.Session.GetString("JWToken");

            if (!IsValidToken(token))
            {
                context.Session.Remove("JWToken");
                context.Items["ApiMessage"] = "Your token has expired.";
                context.Items["ApiType"] = "danger";

                context.Response.Redirect("/Auth/Login");
                return;
            }

            await _next(context);
        }




        private bool IsValidToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            try
            {
                var handler = new JwtSecurityTokenHandler();

                if (!handler.CanReadToken(token))
                    return false;

                var jwt = handler.ReadJwtToken(token);

                var expClaim = jwt.Claims.FirstOrDefault(c => c.Type == "exp");
                if (expClaim != null)
                {
                    var exp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim.Value));
                    return exp > DateTimeOffset.UtcNow;
                }

                return true; // Si no tiene exp, considerarlo válido
            }
            catch
            {
                return false;
            }
        }
    }
}
