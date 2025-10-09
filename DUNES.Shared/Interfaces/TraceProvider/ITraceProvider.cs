namespace DUNES.API.Utils.TraceProvider
{

    /// <summary>
    /// created trace id http request
    /// </summary>
    public interface ITraceProvider
    {
        /// <summary>
        /// return a trace id generated
        /// </summary>
        string TraceId { get; }
    }
}
