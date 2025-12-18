using DUNES.API.ModelsWMS.Masters;

namespace DUNES.API.RepositoriesWMS.Masters.CompaniesClientDivision
{
    /// <summary>
    /// company client division repository CUD (Create,Update, Delete)
    /// </summary>
    public interface ICommandCompaniesClientDivisionWMSAPIRepository
    {
        

        /// <summary>
        /// add new client company division
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<CompanyClientDivision> AddClientCompanyDivisionAsync(CompanyClientDivision entity, CancellationToken ct);

        /// <summary>
        /// update client company division
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> UpdateClientCompanyDivisionAsync(CompanyClientDivision entity, CancellationToken ct);

        /// <summary>
        /// delete client company division
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> DeleteClientCompanyDivisionAsync(int id, CancellationToken ct);
    }
}
