namespace DUNES.API.ServicesWMS.Admin
{

    /// <summary>
    /// save error (try - catch) in database table
    /// </summary>
    public interface IErrorLogService
    {
        /// <summary>
        /// save error catch
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        /// <param name="origin"></param>
        /// <param name="overrideMessage"></param>
        /// <returns></returns>
        Task TrySaveAsync(HttpContext context, Exception ex, string origin, string? overrideMessage = null);
    }
}
