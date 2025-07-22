namespace APIZEBRA.Utils.Responses
{
    /// <summary>
    /// Standard response for all API end poits.
    /// </summary>
    public class ApiResponse<T>
    {
        /// <summary>
        /// A general message describing the outcome of the operation.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Detailed error message if the operation failed.
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// The main content or payload returned by the operation.
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// The date and time when the response was processed.
        /// </summary>
        public DateTime ProcessDate { get; set; } = DateTime.Now;

        /// <summary>
        /// The HTTP status code or internal code representing the result.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// A unique identifier used for tracing the operation.
        /// </summary>
        public string? TraceId { get; set; }

        /// <summary>
        /// Indicates whether the operation was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// A list of additional warnings or non-critical issues.
        /// </summary>
        public List<string> Warnings { get; set; } = new(); // Always initialized



    }
}
