using DUNES.Shared.DTOs.Auth;
using DUNES.UI.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace DUNES.UI.Middleware
{
    /// <summary>
    /// Middleware encargado de validar la sesión del usuario
    /// basada en SessionDTO y el JWT almacenado en sesión.
    /// </summary>
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITempDataDictionaryFactory _tempDataFactory;

        public TokenValidationMiddleware(
            RequestDelegate next,
            ITempDataDictionaryFactory tempDataFactory)
        {
            _next = next;
            _tempDataFactory = tempDataFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLowerInvariant() ?? "";

            // ✅ Rutas públicas que no requieren sesión
            var excluded = new[]
            {
                "/auth/login",
                "/auth/logout",
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

            // 🔐 Obtener sesión completa
            var sessionJson = context.Session.GetString("UserSession");

            if (string.IsNullOrWhiteSpace(sessionJson))
            {
                RedirectToLogin(context, "Session not found. Please login again.");
                return;
            }

            SessionDTO? session;

            try
            {
                session = JsonSerializer.Deserialize<SessionDTO>(sessionJson);
            }
            catch
            {
                RedirectToLogin(context, "Invalid session data.");
                return;
            }

            if (session == null || string.IsNullOrWhiteSpace(session.Token))
            {
                RedirectToLogin(context, "Invalid session token.");
                return;
            }

            if (!IsValidToken(session.Token))
            {
                context.Session.Remove("UserSession");
                RedirectToLogin(context, "Your session has expired.");
                return;
            }

            await _next(context);
        }

        /// <summary>
        /// Valida la expiración del JWT
        /// </summary>
        private static bool IsValidToken(string token)
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
                if (expClaim != null &&
                    long.TryParse(expClaim.Value, out var expSeconds))
                {
                    var exp = DateTimeOffset.FromUnixTimeSeconds(expSeconds);
                    return exp > DateTimeOffset.UtcNow;
                }

                // Si no tiene exp, lo consideramos válido
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Redirige al login con mensaje
        /// </summary>
        private static void RedirectToLogin(HttpContext context, string message)
        {
            context.Items["ApiMessage"] = message;
            context.Items["ApiType"] = "danger";
            context.Response.Redirect("/Auth/Login");
        }
    }
}
