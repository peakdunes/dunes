namespace DUNES.API.Models
{
    /// <summary>
    /// Log exception error
    /// </summary>
    public class DbkMvcLogApi
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty; 
        public string Exception { get; set; } = string.Empty;   
        public string Level { get; set; } = string.Empty;   
        public DateTime TimeStamp { get; set; }
        public string Usuario { get; set; } = string.Empty; 
        public string Origen { get; set; } = string.Empty;  
        public string Ruta { get; set; } = string.Empty;    

    }
}
