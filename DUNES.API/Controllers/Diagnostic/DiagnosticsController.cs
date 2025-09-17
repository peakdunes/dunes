using DUNES.API.Data;
using DUNES.API.Utils.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Controllers.Diagnostic
{
    /// <summary>
    /// API diagnostic
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosticController : BaseController
    {
        private readonly AppDbContext _context;


        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="context"></param>
        public DiagnosticController(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// ping test
        /// </summary>
        /// <returns></returns>
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Respond.Ok("✅ API is up and running.");
        }

        /// <summary>
        /// Health check - Verifies that the database connection is alive
        /// </summary>
        /// <returns></returns>
        [HttpGet("db-connection")]
        public async Task<IActionResult> CheckDbConnection()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                if (!canConnect)
                {
                    return Respond.InternalError("❌ Unable to connect to the database.");
                }

                return Respond.Ok("✅ Successfully connected to the database.");
            }
            catch (Exception ex)
            {
                return Respond.InternalError($"❌ Database connection error: {ex.Message}");
            }
        }

        /// <summary>
        /// 200 OK
        /// </summary>
        /// <returns></returns>
        [HttpGet("ok")]
        public IActionResult GetOk() =>
            Respond.Ok("✅ Operation completed successfully.");

        /// <summary>
        ///  201 Created
        /// </summary>
        /// <returns></returns>
        [HttpGet("created")]
        public IActionResult GetCreated()
        {
            var data = new { Id = 1001, Model = "TC58", Status = "New" };
            return Respond.Created(data, "✅ Resource created successfully.");
        }

        /// <summary>
        /// 204 No Content
        /// </summary>
        /// <returns></returns>
        [HttpGet("nocontent")]
        public IActionResult GetNoContent() =>
            Respond.NoContent("✅ Request processed successfully. No content to return.");

        /// <summary>
        /// 400 Bad Request
        /// </summary>
        /// <returns></returns>
        [HttpGet("badrequest")]
        public IActionResult GetBadRequest() =>
            Respond.BadRequest("❌ Invalid or missing parameters.");

        /// <summary>
        /// 401 Unauthorized
        /// </summary>
        /// <returns></returns>
        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorized() =>
            Respond.Unauthorized("🔒 Authentication token is missing or invalid.");

        /// <summary>
        /// 403 Forbidden
        /// </summary>
        /// <returns></returns>
        [HttpGet("forbidden")]
        public IActionResult GetForbidden() =>
            Respond.Forbidden("🚫 You do not have permission to access this resource.");

        /// <summary>
        /// 404 Not Found
        /// </summary>
        /// <returns></returns>
        [HttpGet("notfound")]
        public IActionResult GetNotFound() =>
            Respond.NotFound("❌ The requested resource was not found.");

        /// <summary>
        /// 409 Conflict
        /// </summary>
        /// <returns></returns>
        [HttpGet("conflict")]
        public IActionResult GetConflict() =>
            Respond.Conflict("⚠️ A resource conflict occurred (e.g., duplicate entry).");

        /// <summary>
        /// 422 Unprocessable Entity
        /// </summary>
        /// <returns></returns>
        [HttpGet("unprocessable")]
        public IActionResult GetUnprocessable() =>
            Respond.Unprocessable("📛 One or more fields have invalid formatting.");

        /// <summary>
        /// 500 Internal Server Error (manual)
        /// </summary>
        /// <returns></returns>
        [HttpGet("internalerror")]
        public IActionResult GetInternalError() =>
            Respond.InternalError("💥 An unexpected error occurred during processing.");

        /// <summary>
        /// 500 Forced exception (to test middleware, logs, etc.)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("force-error")]
        public IActionResult ForceError() =>
            throw new Exception("💥 Forced error to trigger middleware and Serilog logging.");

        /// <summary>
        /// Retrieve latest Serilog entries from the DB
        /// </summary>
        /// <returns></returns>
        [HttpGet("logs")]
        public async Task<IActionResult> GetLogs()
        {
            var logs = await _context.DbkMvcLogApi
                .OrderByDescending(l => l.TimeStamp)
                .Take(10)
                .Select(l => new
                {
                    l.TimeStamp,
                    l.Level,
                    l.Message,
                    l.Exception,
                    l.Ruta,
                    l.Origen,
                    l.Usuario
                })
                .ToListAsync();

            return Respond.Ok(logs, "✅ Retrieved last 10 API log entries.");
        }


    }
}
