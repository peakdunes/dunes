using DUNES.API.Data;
using DUNES.API.ModelsWMS.Masters;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.CompaniesContract
{


    /// <summary>
    /// companies client contract
    /// </summary>
    public class CompaniesContractWMSAPIRepository : ICompaniesContractWMSAPIRepository
    {


        private readonly appWmsDbContext _context;


        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="context"></param>

        public CompaniesContractWMSAPIRepository(appWmsDbContext context)
        {

            _context = context;
        }

        /// <summary>
        /// add client company contract
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<API.ModelsWMS.Masters.CompaniesContract> AddClientCompanyContractAsync(API.ModelsWMS.Masters.CompaniesContract entity, CancellationToken ct)
        {
            _context.CompaniesContract.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
        /// <summary>
        /// delete company client contract by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteClientCompanyContractAsync(int id, CancellationToken ct)
        {
            var infoclient = await _context.CompaniesContract.FirstOrDefaultAsync(x => x.Id == id);

            if (infoclient == null)
            {
                return false;
            }

            _context.CompaniesContract.Remove(infoclient);
            await _context.SaveChangesAsync();

            return true;

        }



        /// <summary>
        /// Get all client company contract information
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<API.ModelsWMS.Masters.CompaniesContract>> GetAllClientCompaniesContractInformationAsync(CancellationToken ct)
        {
            var companyclientlist = await _context.CompaniesContract
               .Include(x => x.CompanyClientNavegation)
               .Include(x => x.CompanyNavegation)
                .ToListAsync();

            return companyclientlist;


        }
        /// <summary>
        /// Get all client information contract by contract number
        /// </summary>
        /// <param name="contractcode"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<API.ModelsWMS.Masters.CompaniesContract> GetClientCompanyInformationContractByNumberAsync(string contractcode, CancellationToken ct)
        {
            var infocontract = await _context.CompaniesContract
               .Include(x => x.CompanyClientNavegation)
               .Include(x => x.CompanyNavegation)
                .FirstOrDefaultAsync(x => x.ContractCode == contractcode);

            return infocontract;
        }


        /// <summary>
        /// Get all client contract information for a contract by id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        ///     



        public async Task<API.ModelsWMS.Masters.CompaniesContract> GetClientCompanyContractInformationByIdAsync(int Id, CancellationToken ct)
        {
            var infoclient = await _context.CompaniesContract
               .Include(x => x.CompanyClientNavegation)
               .Include(x => x.CompanyNavegation)
                .FirstOrDefaultAsync(x => x.Id == Id);

            return infoclient;
        }



        /// <summary>
        /// update company client contract by id
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> UpdateClientCompanyContractAsync(CompanyClient entity, CancellationToken ct)
        {
            var infoclient = await _context.CompaniesContract.FirstOrDefaultAsync(x => x.Id == entity.Id);

            if (infoclient == null)
            {
                return false;
            }

            _context.CompaniesContract.Update(infoclient);
            await _context.SaveChangesAsync();

            return true;
        }

     
    }
}
