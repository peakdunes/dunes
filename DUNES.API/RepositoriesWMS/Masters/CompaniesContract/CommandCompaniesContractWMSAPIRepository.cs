using DUNES.API.Data;
using DUNES.API.ModelsWMS.Masters;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.CompaniesContract
{

    /// <summary>
    /// company client contract CRUD
    /// </summary>
    public class CommandCompaniesContractWMSAPIRepository : ICommandCompaniesContractWMSAPIRepository
    {
        /// <summary>
        /// DI
        /// </summary>
        public readonly appWmsDbContext _context;


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="context"></param>
        public CommandCompaniesContractWMSAPIRepository(appWmsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// add new contract
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ModelsWMS.Masters.CompaniesContract> AddClientCompanyContractAsync(ModelsWMS.Masters.CompaniesContract entity, CancellationToken ct)
        {
           _context.CompaniesContract.Add(entity);

            await _context.SaveChangesAsync();

            return entity;
            
            
        }
        /// <summary>
        /// delete contract
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteClientCompanyContractAsync(int id, CancellationToken ct)
        {
            var entity = await _context.CompaniesContract.FirstOrDefaultAsync(x => x.Id == id);

            if (entity != null)
            {
                _context.CompaniesContract.Remove(entity);

                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }

               
        }
        /// <summary>
        /// update contract
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> UpdateClientCompanyContractAsync(ModelsWMS.Masters.CompaniesContract entity, CancellationToken ct)
        {
            _context.CompaniesContract.Update(entity);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
