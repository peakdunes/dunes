using DUNES.API.ModelsWMS.Masters;
using DUNES.API.Repositories.Inventory.ASN.Queries;
using DUNES.API.RepositoriesWMS.Masters;
using DUNES.API.Utils.Responses;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters
{
    /// <summary>
    /// All common Master Tables queries
    /// </summary>
    public class CommonQueryWMSMasterService : ICommonQueryWMSMasterService
    {

        private readonly ICommonQueryWMSMasterRepository _repository;


        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="repository"></param>
        public CommonQueryWMSMasterService(ICommonQueryWMSMasterRepository repository)
        {
            _repository = repository;
        }



        /// <summary>
        /// Get all information for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Company>> GetCompanyInformation(int companyid)
        {

            var infocompany = await _repository.GetCompanyInformation(companyid);

            if (infocompany == null)
                return ApiResponseFactory.BadRequest<Company>("Company information not found");


            return ApiResponseFactory.Ok(infocompany, "OK");
        }

        /// <summary>
        /// Get all locations for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Locations>>> GetAllLocationsByCompany(int companyid)
        {
            var infoloc = await _repository.GetAllLocationsByCompany(companyid);

            if (infoloc.Count <= 0)
                return ApiResponseFactory.BadRequest<List<Locations>>($"Locations for this company {companyid} not found");


            return ApiResponseFactory.Ok(infoloc, "OK");
        }

        /// <summary>
        /// Get all inventory type for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<InventoryTypes>>> GetAllInventoryTypesByCompanyClient(int companyid, string companyClient)
        {
            var infotypes = await _repository.GetAllInventoryTypesByCompanyClient(companyid, companyClient);

            if (infotypes.Count <= 0)
                return ApiResponseFactory.BadRequest<List<InventoryTypes>>($"Inventory types for this company client {companyClient} not found");

            return ApiResponseFactory.Ok(infotypes, "OK");
        }

        /// <summary>
        /// Get all actives inventory type for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<InventoryTypes>>> GetAllActiveInventoryTypesByCompanyClient(int companyid, string companyClient)
        {
            var infotypes = await _repository.GetAllActiveInventoryTypesByCompanyClient(companyid, companyClient);

            if (infotypes.Count <= 0)
                return ApiResponseFactory.BadRequest<List<InventoryTypes>>($"Inventory types actives for this company client {companyClient} not found");

            return ApiResponseFactory.Ok(infotypes, "OK");
        }



        /// <summary>
        /// Get all item status for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Itemstatus>>> GetAllItemStatusByCompanyClient(int companyid, string companyClient)
        {
            var infostatus = await _repository.GetAllItemStatusByCompanyClient(companyid, companyClient);

            if (infostatus.Count <= 0)
                return ApiResponseFactory.BadRequest<List<Itemstatus>>($"Item status for this company client {companyClient} not found");

            return ApiResponseFactory.Ok(infostatus, "OK");
        }

        /// <summary>
        /// Get all actives inventory type for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Itemstatus>>> GetAllActiveItemStatusByCompanyClient(int companyid, string companyClient)
        {
            var infostatus = await _repository.GetAllActiveItemStatusByCompanyClient(companyid, companyClient);

            if (infostatus.Count <= 0)
                return ApiResponseFactory.BadRequest<List<Itemstatus>>($"Item status actives for this company client {companyClient} not found");

            return ApiResponseFactory.Ok(infostatus, "OK");
        }
        /// <summary>
        /// Get all Racks for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Racks>>> GetAllRacksByCompanyClient(int companyid, string companyClient)
        {
            var inforacks = await _repository.GetAllRacksByCompanyClient(companyid, companyClient);

            if (inforacks.Count <= 0)
                return ApiResponseFactory.BadRequest<List<Racks>>($"Racks for this company client {companyClient} not found");

            return ApiResponseFactory.Ok(inforacks, "OK");
        }
        /// <summary>
        /// Get all actives Racks for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Racks>>> GetAllActiveRacksByCompanyClient(int companyid, string companyClient)
        {
            var inforacks = await _repository.GetAllActiveRacksByCompanyClient(companyid, companyClient);

            if (inforacks.Count <= 0)
                return ApiResponseFactory.BadRequest<List<Racks>>($"Racks actives for this company client {companyClient} not found");

            return ApiResponseFactory.Ok(inforacks, "OK");
        }
        /// <summary>
        /// Get all Bins for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Bines>>> GetAllBinsByCompanyClient(int companyid, string companyClient)
        {
            var infobins = await _repository.GetAllBinsByCompanyClient(companyid, companyClient);

            if (infobins.Count <= 0)
                return ApiResponseFactory.BadRequest<List<Bines>>($"Bins for this company client {companyClient} not found");

            return ApiResponseFactory.Ok(infobins, "OK");
        }
        /// <summary>
        /// Get all actives Bins for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Bines>>> GetAllActiveBinsByCompanyClient(int companyid, string companyClient)
        {
            var infobins = await _repository.GetAllActiveBinsByCompanyClient(companyid, companyClient);

            if (infobins.Count <= 0)
                return ApiResponseFactory.BadRequest<List<Bines>>($"Bins actives for this company client {companyClient} not found");

            return ApiResponseFactory.Ok(infobins, "OK");
        }
    }
}
