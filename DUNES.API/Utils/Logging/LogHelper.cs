using DUNES.API.Data;
using DUNES.API.Models;
using Microsoft.Data.SqlClient;

namespace DUNES.API.Utils.Logging
{
    public class LogHelper
    {
        private readonly IConfiguration _config;

        public LogHelper(IConfiguration config)
        {
            _config = config;
        }

        public async Task SaveLogAsync(
            string traceId,
            string message,
            string exception,
            string level,
            string usuario,
            string origen,
            string ruta)
        {
            var connectionString = _config.GetConnectionString("DefaultConnection");

            using var conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            // 🔹 Sanitizamos el mensaje y la excepción para evitar errores por longitud
            string cleanMessage = string.IsNullOrEmpty(message)
                ? string.Empty
                : message.Split('\n')[0].Trim();  // Solo la primera línea clara

            if (cleanMessage.Length > 500)
                cleanMessage = cleanMessage.Substring(0, 500);  // Cortamos a 500 si es muy largo

            string cleanException = string.IsNullOrEmpty(exception)
                ? string.Empty
                : exception.Split('\n')[0].Trim();  // Solo la primera línea clara

            if (cleanException.Length > 1000)
                cleanException = cleanException.Substring(0, 1000);  // Cortamos a 1000 si es muy largo



            var query = @"
            INSERT INTO dbk_mvc_logs_api (Message, Exception, Level, Usuario, Origen, Ruta, TimeStamp, TraceId)
            VALUES (@Message, @Exception, @Level, @Usuario, @Origen, @Ruta, GETDATE(), @TraceId)";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@TraceId", traceId);
            cmd.Parameters.AddWithValue("@Message", cleanMessage);
            cmd.Parameters.AddWithValue("@Exception", (object?)cleanException ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Level", level);
            cmd.Parameters.AddWithValue("@Usuario", (object?)usuario ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Origen", origen);
            cmd.Parameters.AddWithValue("@Ruta", (object?)ruta ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
