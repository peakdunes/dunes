using APIZEBRA.Data;
using APIZEBRA.Models;
using Microsoft.Data.SqlClient;

namespace APIZEBRA.Utils.Logging
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

            var query = @"
            INSERT INTO dbk_mvc_logs_api (Message, Exception, Level, Usuario, Origen, Ruta, TimeStamp, TraceId)
            VALUES (@Message, @Exception, @Level, @Usuario, @Origen, @Ruta, GETDATE(), @TraceId)";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@TraceId", Guid.Parse(traceId));
            cmd.Parameters.AddWithValue("@Message", message);
            cmd.Parameters.AddWithValue("@Exception", (object?)exception ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Level", level);
            cmd.Parameters.AddWithValue("@Usuario", (object?)usuario ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Origen", origen);
            cmd.Parameters.AddWithValue("@Ruta", (object?)ruta ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
