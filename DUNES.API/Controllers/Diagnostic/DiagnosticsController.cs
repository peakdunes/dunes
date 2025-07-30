using DUNES.API.Data;
using DUNES.API.Utils.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Controllers.Diagnostic
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosticController : BaseController
    {
        private readonly AppDbContext _context;

        public DiagnosticController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Respond.Ok("✅ API is up and running.");
        }

        // Health check - Verifies that the database connection is alive
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

        // 200 OK
        [HttpGet("ok")]
        public IActionResult GetOk() =>
            Respond.Ok("✅ Operation completed successfully.");

        // 201 Created
        [HttpGet("created")]
        public IActionResult GetCreated()
        {
            var data = new { Id = 1001, Model = "TC58", Status = "New" };
            return Respond.Created(data, "✅ Resource created successfully.");
        }

        // 204 No Content
        [HttpGet("nocontent")]
        public IActionResult GetNoContent() =>
            Respond.NoContent("✅ Request processed successfully. No content to return.");

        // 400 Bad Request
        [HttpGet("badrequest")]
        public IActionResult GetBadRequest() =>
            Respond.BadRequest("❌ Invalid or missing parameters.");

        // 401 Unauthorized
        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorized() =>
            Respond.Unauthorized("🔒 Authentication token is missing or invalid.");

        // 403 Forbidden
        [HttpGet("forbidden")]
        public IActionResult GetForbidden() =>
            Respond.Forbidden("🚫 You do not have permission to access this resource.");

        // 404 Not Found
        [HttpGet("notfound")]
        public IActionResult GetNotFound() =>
            Respond.NotFound("❌ The requested resource was not found.");

        // 409 Conflict
        [HttpGet("conflict")]
        public IActionResult GetConflict() =>
            Respond.Conflict("⚠️ A resource conflict occurred (e.g., duplicate entry).");

        // 422 Unprocessable Entity
        [HttpGet("unprocessable")]
        public IActionResult GetUnprocessable() =>
            Respond.Unprocessable("📛 One or more fields have invalid formatting.");

        // 500 Internal Server Error (manual)
        [HttpGet("internalerror")]
        public IActionResult GetInternalError() =>
            Respond.InternalError("💥 An unexpected error occurred during processing.");

        // 500 Forced exception (to test middleware, logs, etc.)
        [HttpGet("force-error")]
        public IActionResult ForceError() =>
            throw new Exception("💥 Forced error to trigger middleware and Serilog logging.");

        // Retrieve latest Serilog entries from the DB
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
