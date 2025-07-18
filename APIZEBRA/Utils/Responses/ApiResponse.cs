namespace APIZEBRA.Utils.Responses
{
    public class ApiResponse<T>
    {
        public string Message { get; set; } = string.Empty;
        public string? Error { get; set; }
        public T? Data { get; set; }
        public DateTime ProcessDate { get; set; } = DateTime.Now;
        public int StatusCode { get; set; }
        public string? TraceId { get; set; }
        public bool Success { get; set; }
        public List<string> Warnings { get; set; } = new(); // Siempre inicializada



    }
}
