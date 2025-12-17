using DUNES.API.Data;
using DUNES.API.ModelsWMS.Masters;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace DUNES.API.RepositoriesWMS.Masters.ClientCompanies
{

    /// <summary>
    /// Client Companies Repository Implementation
    /// </summary>
    public class ClientCompaniesWMSAPIRepository : IClientCompaniesWMSAPIRepository
    {



        private readonly appWmsDbContext _context;


        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="context"></param>

        public ClientCompaniesWMSAPIRepository(appWmsDbContext context)
        {
            
            _context = context;
        }

        /// <summary>
        /// add client company
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CompanyClient> AddClientCompanyAsync(CompanyClient entity, CancellationToken ct)
        {
            _context.CompanyClient.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
        /// <summary>
        /// delete company client by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteClientCompanyAsync(int id, CancellationToken ct)
        {
            var infoclient = await _context.CompanyClient.FirstOrDefaultAsync(x => x.Id == id);

            if (infoclient == null)
            {
                return false;
            }

            _context.CompanyClient.Remove(infoclient);
            await _context.SaveChangesAsync();

            return true;

        }



        /// <summary>
        /// Get all client company  information
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<CompanyClient>> GetAllClientCompaniesInformationAsync(CancellationToken ct)
        {
            var companyclientlist = await _context.CompanyClient
               .Include(x => x.CountryNavegation)
               .Include(x => x.StateNavegation)
               .Include(x => x.CityNavegation)
                .ToListAsync();

            return companyclientlist;


        }
        /// <summary>
        /// Get all client information for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<CompanyClient> GetClientCompanyInformationByIdentificationAsync(string companyid, CancellationToken ct)
        {
            var infoclient = await _context.CompanyClient
               .Include(x => x.CountryNavegation)
               .Include(x => x.StateNavegation)
               .Include(x => x.CityNavegation)
                .FirstOrDefaultAsync(x => x.CompanyId == companyid);

            return infoclient;
        }


        /// <summary>
        /// Get all client information for a company by id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        ///                            
        public async Task<CompanyClient> GetClientCompanyInformationByIdAsync(int Id, CancellationToken ct)
        {
            var infoclient = await _context.CompanyClient
              .Include(x => x.CountryNavegation)
               .Include(x => x.StateNavegation)
               .Include(x => x.CityNavegation)
                .FirstOrDefaultAsync(x => x.Id == Id);

            return infoclient;
        }

      

        /// <summary>
        /// update company client by id
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> UpdateClientCompanyAsync(CompanyClient entity, CancellationToken ct)
        {
            var infoclient = await _context.CompanyClient.FirstOrDefaultAsync(x => x.Id == entity.Id);

            if (infoclient == null)
            {
                return false;
            }

            _context.CompanyClient.Update(infoclient);
            await _context.SaveChangesAsync();

            return true;
        }
        /// <summary>
        /// get company information by name
        /// </summary>
        /// <param name="companyname"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<CompanyClient> GetClientCompanyInformationByNameAsync(string companyname, CancellationToken ct)
        {
            var infoclient = await _context.CompanyClient
            .Include(x => x.CountryNavegation)
               .Include(x => x.StateNavegation)
               .Include(x => x.CityNavegation)
                .FirstOrDefaultAsync(x => x.Name!.ToUpper() == companyname.ToUpper());

            return infoclient;
        }
    }
}
