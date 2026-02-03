using DUNES.API.RepositoriesWMS.Masters.Companies;

namespace DUNES.API.Utils.Middlewares
{
    /// <summary>
    /// valida que todos los requests envien el claim del companyid
    /// </summary>
    public class CompanyContextMiddleware
    {

        private readonly RequestDelegate _next;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="next"></param>
        public CompanyContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        /// <summary>
        /// check for a companyid claim
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            // Solo validar si está autenticado
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var companyClaim = context.User.FindFirst("companyId");
                var companyClientClaim = context.User.FindFirst("companyClientId");

                if (companyClaim == null || companyClientClaim == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync(
                        "Company or CompanyClient context is missing."
                    );
                    return;
                }

                if (!int.TryParse(companyClaim.Value, out var companyId) || companyId <= 0 ||
                    !int.TryParse(companyClientClaim.Value, out var companyClientId) || companyClientId <= 0)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync(
                        "Invalid company or company client context."
                    );
                    return;
                }

                var ct = context.RequestAborted;

                // ✅ Resolver repo desde el RequestServices (scoped)
                var companiesRepo = context.RequestServices.GetRequiredService<ICompaniesWMSAPIRepository>();

                var companyIsActive = await companiesRepo.IsActiveAsync(companyId, ct);

                if (!companyIsActive)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Company does not exist or is inactive.");
                    return;
                }


            }

            await _next(context);
        }
    }
}
