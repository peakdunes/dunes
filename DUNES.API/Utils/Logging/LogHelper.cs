using DUNES.API.Data;
using DUNES.API.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DUNES.API.Utils.Logging
{
    /// <summary>
    /// Transaction Logs
    /// </summary>
    public class LogHelper
    {
        private readonly IConfiguration _config;


        /// <summary>
        /// constructor (DI)
        /// </summary>
        /// <param name="config"></param>
        public LogHelper(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Guarda un log en la tabla dbk_mvc_logs_api
        /// </summary>
        public async Task SaveLogAsync(
            string traceId,
            string message,
            string? exception,
            string level,
            string? usuario,
            string origen,
            string? ruta,
            CancellationToken ct = default)
        {
            try
            {
              
                var cs = _config.GetConnectionString("DefaultWMSConnection");

                using var conn = new SqlConnection(cs);
                await conn.OpenAsync(ct);

                // Sanitiza 1 línea clara y limita longitud
                string cleanMessage = string.IsNullOrWhiteSpace(message)
                    ? string.Empty
                    : message.Split('\n')[0].Trim();

                if (cleanMessage.Length > 500)
                    cleanMessage = cleanMessage[..500];

                string? cleanException = string.IsNullOrWhiteSpace(exception)
                    ? null
                    : exception.Split('\n')[0].Trim();

                if (cleanException is { Length: > 1000 })
                    cleanException = cleanException[..1000];

                const string sql = @"
                        INSERT INTO dbk_mvc_logs_api (Message, Exception, Level, Usuario, Origen, Ruta, TimeStamp, TraceId)
                        VALUES (@Message, @Exception, @Level, @Usuario, @Origen, @Ruta, GETUTCDATE(), @TraceId);";

                using var cmd = new SqlCommand(sql, conn);

                // Parámetros tipados y con tamaño
                cmd.Parameters.Add(new SqlParameter("@TraceId", SqlDbType.NVarChar, 100) { Value = traceId });
                cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar, 500) { Value = (object)cleanMessage ?? DBNull.Value });
                cmd.Parameters.Add(new SqlParameter("@Exception", SqlDbType.NVarChar, 1000) { Value = (object?)cleanException ?? DBNull.Value });
                cmd.Parameters.Add(new SqlParameter("@Level", SqlDbType.NVarChar, 20) { Value = level });
                cmd.Parameters.Add(new SqlParameter("@Usuario", SqlDbType.NVarChar, 100) { Value = (object?)usuario ?? DBNull.Value });
                cmd.Parameters.Add(new SqlParameter("@Origen", SqlDbType.NVarChar, 200) { Value = origen });
                cmd.Parameters.Add(new SqlParameter("@Ruta", SqlDbType.NVarChar, 500) { Value = (object?)ruta ?? DBNull.Value });

                await cmd.ExecuteNonQueryAsync(ct);
            }
            catch (Exception ex)
            {
                // Fallback: escribe el fallo del logger en el archivo de Serilog (no re-lanzar)
                try
                {
                    Serilog.Log.Error(ex,
                        "[LOGHELPER] Falló guardando log en DB (se ignora). TraceId={TraceId} Origen={Origen} Ruta={Ruta}",
                        traceId, origen, ruta);
                }
                catch
                {
                    // Última línea de defensa: nunca dejes que el logger rompa el flujo
                }
            }
        }
    }
}
