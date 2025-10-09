using Microsoft.AspNetCore.Http;

namespace DUNES.API.Utils.TraceProvider
{

    /// <summary>
    ///  create trace id http request
    /// </summary>
    public class TraceProvider : ITraceProvider
    {
        private readonly IHttpContextAccessor _http;


        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="http"></param>
        public TraceProvider(IHttpContextAccessor http) => _http = http;

        /// <summary>
        /// generate a TRACE_ID by http request
        /// </summary>
        public string TraceId =>
            (_http.HttpContext?.Items["__TraceId"] as string)
            ?? _http.HttpContext?.TraceIdentifier
            ?? Guid.NewGuid().ToString("N"); // solo si no hay HttpContext (jobs)
    }
}
