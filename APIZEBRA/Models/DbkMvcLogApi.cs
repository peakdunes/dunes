namespace APIZEBRA.Models
{
    /// <summary>
    /// Log exception error
    /// </summary>
    public class DbkMvcLogApi
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string Level { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Usuario { get; set; }
        public string Origen { get; set; }
        public string Ruta { get; set; }

    }
}
