namespace DUNES.UI.Models
{
    /// <summary>
    /// Standard UI message
    /// </summary>
    public class UIMsg
    {
        /// <summary>
        /// type message success, error, warning, info
        /// </summary>
        public string Type { get; set; }   
        /// <summary>
        /// text message
        /// </summary>
        public string Message { get; set; }
    }
}
