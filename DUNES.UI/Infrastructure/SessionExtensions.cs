using DUNES.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Components.Forms;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DUNES.UI.Infrastructure
{
//    Extiende HttpContext o ISession

//👉 Se usa después del login
//👉 Para leer datos persistidos de sesión
//👉 También vive en Infrastructure

//Ejemplo:

    //var session = HttpContext.GetUserSession();
    //var token = session.Token;

//📌 Responsabilidad:

    //Acceso seguro y tipado a Session
    //Evitar Session.GetString("JWToken") por todos lados
    //Centralizar validación de sesión

    public static class SessionExtensions
    {

        //UserSession es el nombre que se le dio al objeto de session que
        //guarda toda la informacion cuando el usuario se autentisa
        //UserId, TOken, CompanyDefault, CompanyClientDefault, LocationDefault
        //HttpContext.Session.SetString("UserSession",JsonSerializer.Serialize(session));

        private const string SESSION_KEY = "UserSession";

        public static SessionDTO GetUserSession(this HttpContext context)
        {
            var json = context.Session.GetString(SESSION_KEY);

            if (string.IsNullOrWhiteSpace(json))
                return null;

            return JsonSerializer.Deserialize<SessionDTO>(json);
        }
        public static SessionDTO GetUserSessionOrThrow(this HttpContext context)
        {
            var session = context.GetUserSession();

            if (session == null)
                throw new UnauthorizedAccessException("User session not found.");

            return session;
        }
    }
}
