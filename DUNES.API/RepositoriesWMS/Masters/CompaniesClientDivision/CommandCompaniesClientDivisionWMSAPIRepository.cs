using DUNES.API.Data;
using DUNES.API.ModelsWMS.Masters;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.CompaniesClientDivision
{
    /// <summary>
    /// company client division repository CUD (Create,Update, Delete)
    /// </summary>
    public class CommandCompaniesClientDivisionWMSAPIRepository : ICommandCompaniesClientDivisionWMSAPIRepository
    {

        private readonly appWmsDbContext _dbContext;

        /// <summary>
        /// contructor
        /// </summary>
        /// <param name="dbContext"></param>
        public CommandCompaniesClientDivisionWMSAPIRepository(appWmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// add new company client division
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CompanyClientDivision> AddClientCompanyDivisionAsync(CompanyClientDivision entity, CancellationToken ct)
        {

            _dbContext.CompanyClientDivision.Add(entity);
            await _dbContext.SaveChangesAsync(ct);

            return entity;

           
        }
        /// <summary>
        /// delete company client division
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteClientCompanyDivisionAsync(int id, CancellationToken ct)
        {
            var infodel = await _dbContext.CompanyClientDivision.FirstOrDefaultAsync(x => x.Id == id);

            _dbContext.CompanyClientDivision.Remove(infodel!);
          

            return true;
        }
        /// <summary>
        /// update company client division
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> UpdateClientCompanyDivisionAsync(CompanyClientDivision entity, CancellationToken ct)
        {
            _dbContext.CompanyClientDivision.Update(entity);
            await _dbContext.SaveChangesAsync(ct);

            return true;
        }
    }
}
