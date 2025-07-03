using APIZEBRA.Data;
using APIZEBRA.Models;

namespace APIZEBRA.Utils.Logging
{
    public class LogHelper
    {
        private readonly AppDbContext _context;

        public LogHelper(AppDbContext context)
        {
            _context = context;
        }

        public async Task LogErrorAsync(Exception ex, string origen, string ruta, string usuario = "system", string nivel = "Error")
        {
            try
            {
                var mensajeLimpio = ex.GetBaseException().Message.Split('\n')[0].Trim();

                var log = new DbkMvcLogApi
                {
                    Message = $"❌ {mensajeLimpio}",
                    Exception = mensajeLimpio,
                    Level = nivel,
                    TimeStamp = DateTime.Now,
                    Usuario = usuario,
                    Origen = origen,
                    Ruta = ruta
                };

                _context.Add(log);
                await _context.SaveChangesAsync();
            }
            catch
            {

            }
        }

    }
}
