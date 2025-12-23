using DUNES.API.Data;
using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.DTOs.WMS;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.CompaniesClientDivision
{

    /// <summary>
    /// company client division repository
    /// </summary>
    public class QueryCompaniesClientDivisionWMSAPIRepository : IQueryCompaniesClientDivisionWMSAPIRepository
    {



        private readonly appWmsDbContext _context;


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="context"></param>
        public QueryCompaniesClientDivisionWMSAPIRepository(appWmsDbContext context)
        {

            _context = context;
        }



        /// <summary>
        /// get all company client division
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<WMSCompanyClientDivisionReadDTO>> GetAllCompaniesClientDivisionInformation(CancellationToken ct)
        {
            var infodivisionlist = await (from enc in _context.CompanyClientDivision
                                          join det in _context.CompanyClient on
                                          enc.Idcompanyclient equals det.Id
                                          select new WMSCompanyClientDivisionReadDTO
                                          {
                                              Id = enc.Id,
                                              DivisionName = enc.DivisionName,
                                              Idcompanyclient = enc.Idcompanyclient,
                                              IsActive = enc.IsActive,
                                              CompanyClientName = det.Name

                                          }).AsNoTracking().ToListAsync(ct);

            return infodivisionlist;

        }

        /// <summary>
        /// get all company client division by company client
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="companyclientid"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<WMSCompanyClientDivisionReadDTO>> GetAllCompaniesClientDivisionInformationByCompanyClient(int companyclientid, CancellationToken ct)
        {
            var infodivisionlist = await (from enc in _context.CompanyClientDivision
                                          join det in _context.CompanyClient on
                                          enc.Idcompanyclient equals det.Id
                                          where enc.Idcompanyclient == companyclientid
                                          select new WMSCompanyClientDivisionReadDTO
                                          {
                                              Id = enc.Id,
                                              DivisionName = enc.DivisionName,
                                              Idcompanyclient = enc.Idcompanyclient,
                                              IsActive = enc.IsActive,
                                              CompanyClientName = det.Name

                                          }).AsNoTracking().ToListAsync(ct);

            return infodivisionlist;

        }

        /// <summary>
        /// get division by id
        /// </summary>
        /// <param name="divisionId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<WMSCompanyClientDivisionReadDTO?> GetCompanyClientDivisionById(int divisionId, CancellationToken ct)
        {
            var infodivision = await (from enc in _context.CompanyClientDivision
                                      join det in _context.CompanyClient on
                                      enc.Idcompanyclient equals det.Id
                                      where enc.Id == divisionId
                                      select new WMSCompanyClientDivisionReadDTO
                                      {
                                          Id = enc.Id,
                                          DivisionName = enc.DivisionName,
                                          Idcompanyclient = enc.Idcompanyclient,
                                          IsActive = enc.IsActive,
                                          CompanyClientName = det.Name

                                      }).AsNoTracking().FirstOrDefaultAsync(ct);

            return infodivision;
        }


        /// <summary>
        /// get division by name
        /// </summary>
        /// <param name="divisionname"></param>
        /// <param name="ct"></param>
        /// <param name="companyClientId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<WMSCompanyClientDivisionReadDTO?> GetCompanyClientDivisionByNameAsync(int companyClientId, string divisionname, CancellationToken ct)
        {
            var infodivision = await(from enc in _context.CompanyClientDivision
                                     join det in _context.CompanyClient on
                                     enc.Idcompanyclient equals det.Id
                                     where enc.DivisionName!.ToUpper() == divisionname.ToUpper()
                                     && enc.Idcompanyclient == companyClientId
                                     select new WMSCompanyClientDivisionReadDTO
                                     {
                                         Id = enc.Id,
                                         DivisionName = enc.DivisionName,
                                         Idcompanyclient = enc.Idcompanyclient,
                                         IsActive = enc.IsActive,
                                         CompanyClientName = det.Name

                                     }).AsNoTracking().FirstOrDefaultAsync(ct);

            return infodivision;
        }
    }
}
