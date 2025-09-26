using DUNES.API.ModelsWMS.Masters;
using DUNES.API.Repositories.Inventory.ASN.Queries;
using DUNES.API.RepositoriesWMS.Masters;
using DUNES.API.Utils.Responses;
using DUNES.Shared.DTOs.WMS;
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
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Company>> GetCompanyInformation(int companyid, CancellationToken ct)
        {

            var infocompany = await _repository.GetCompanyInformation(companyid, ct);

            if (infocompany == null)
                return ApiResponseFactory.BadRequest<Company>("Company information not found");


            return ApiResponseFactory.Ok(infocompany, "OK");
        }

        /// <summary>
        /// Get all locations for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<WMSLocationsDTO>>> GetAllActiveLocationsByCompany(int companyid, CancellationToken ct)
        {
            var infoloc = await _repository.GetAllActiveLocationsByCompany(companyid,ct);

            if (infoloc.Count <= 0)
                return ApiResponseFactory.BadRequest<List<WMSLocationsDTO>>($"Locations for this company {companyid} not found");

            List<WMSLocationsDTO> locationslist = new List<WMSLocationsDTO>();

            foreach (var location in infoloc)
            {
                WMSLocationsDTO objdet = new WMSLocationsDTO();

                objdet.Id = location.Id;
                objdet.Name = location.Name;

                locationslist.Add(objdet);


            }

            return ApiResponseFactory.Ok(locationslist, "OK");
        }

        /// <summary>
        /// Get all inventory type for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<InventoryTypes>>> GetAllInventoryTypesByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {
            var infotypes = await _repository.GetAllInventoryTypesByCompanyClient(companyid, companyClient, ct);

            if (infotypes.Count <= 0)
                return ApiResponseFactory.BadRequest<List<InventoryTypes>>($"Inventory types for this company client {companyClient} not found");

            return ApiResponseFactory.Ok(infotypes, "OK");
        }

        /// <summary>
        /// Get all actives inventory type for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<InventoryTypes>>> GetAllActiveInventoryTypesByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {
            var infotypes = await _repository.GetAllActiveInventoryTypesByCompanyClient(companyid, companyClient, ct);

            if (infotypes.Count <= 0)
                return ApiResponseFactory.BadRequest<List<InventoryTypes>>($"Inventory types actives for this company client {companyClient} not found");

            return ApiResponseFactory.Ok(infotypes, "OK");
        }



        /// <summary>
        /// Get all item status for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Itemstatus>>> GetAllItemStatusByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {
            var infostatus = await _repository.GetAllItemStatusByCompanyClient(companyid, companyClient, ct);

            if (infostatus.Count <= 0)
                return ApiResponseFactory.BadRequest<List<Itemstatus>>($"Item status for this company client {companyClient} not found");

            return ApiResponseFactory.Ok(infostatus, "OK");
        }

        /// <summary>
        /// Get all actives inventory type for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Itemstatus>>> GetAllActiveItemStatusByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {
            var infostatus = await _repository.GetAllActiveItemStatusByCompanyClient(companyid, companyClient, ct);

            if (infostatus.Count <= 0)
                return ApiResponseFactory.BadRequest<List<Itemstatus>>($"Item status actives for this company client {companyClient} not found");

            return ApiResponseFactory.Ok(infostatus, "OK");
        }
        /// <summary>
        /// Get all Racks for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Racks>>> GetAllRacksByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {
            var inforacks = await _repository.GetAllRacksByCompanyClient(companyid, companyClient, ct);

            if (inforacks.Count <= 0)
                return ApiResponseFactory.BadRequest<List<Racks>>($"Racks for this company client {companyClient} not found");

            return ApiResponseFactory.Ok(inforacks, "OK");
        }
        /// <summary>
        /// Get all actives Racks for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Racks>>> GetAllActiveRacksByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {
            var inforacks = await _repository.GetAllActiveRacksByCompanyClient(companyid, companyClient, ct);

            if (inforacks.Count <= 0)
                return ApiResponseFactory.BadRequest<List<Racks>>($"Racks actives for this company client {companyClient} not found");

            return ApiResponseFactory.Ok(inforacks, "OK");
        }
        /// <summary>
        /// Get all Bins for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Bines>>> GetAllBinsByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {
            var infobins = await _repository.GetAllBinsByCompanyClient(companyid, companyClient, ct);

            if (infobins.Count <= 0)
                return ApiResponseFactory.BadRequest<List<Bines>>($"Bins for this company client {companyClient} not found");

            return ApiResponseFactory.Ok(infobins, "OK");
        }
        /// <summary>
        /// Get all actives Bins for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Bines>>> GetAllActiveBinsByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {
            var infobins = await _repository.GetAllActiveBinsByCompanyClient(companyid, companyClient, ct);

            if (infobins.Count <= 0)
                return ApiResponseFactory.BadRequest<List<Bines>>($"Bins actives for this company client {companyClient} not found");

            return ApiResponseFactory.Ok(infobins, "OK");
        }
        /// <summary>
        /// Get all Company Client actives for a Location id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="locationid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<WMSLocationclientsDTO>>> GetAllActiveClientCompaniesByLocation(int companyid, int locationid, CancellationToken ct)
        {
            var infobins = await _repository.GetAllActiveClientCompaniesByLocation (companyid, locationid, ct);

            if (infobins.Count <= 0)
                return ApiResponseFactory.BadRequest<List<WMSLocationclientsDTO>>($"Company Clients actives for this company {companyid} not found");

            List<WMSLocationclientsDTO> objlist = new List<WMSLocationclientsDTO>();

            foreach (var infob in infobins)
            {
                WMSLocationclientsDTO objdet = new WMSLocationclientsDTO();

                objdet.Idcompanyclient = infob.Idcompanyclient;
                objdet.Idlocation = infob.Idlocation;

                objlist.Add(objdet);

            }

            return ApiResponseFactory.Ok(objlist, "OK");
        }

        /// <summary>
        /// Get all Warehouse Organization for a Client Company
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<WMSWarehouseorganizationDto>>> GetAllWareHouseOrganizationByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {
            var infoorg = await _repository.GetAllWareHouseOrganizationByCompanyClient(companyid, companyClient, ct);

            if (infoorg.Count <= 0)
                return ApiResponseFactory.BadRequest<List<WMSWarehouseorganizationDto>>($"Company Clients don't have a active WareHouse Organization {companyClient} not found");

            List<WMSWarehouseorganizationDto> objlist = new List<WMSWarehouseorganizationDto>();

            foreach (var infob in infoorg)
            {
                WMSWarehouseorganizationDto objdet = new WMSWarehouseorganizationDto();


                objdet.Id = infob.Id;
                objdet.Idcompany = infob.Idcompany;
                objdet.Idcompanyclient = infob.Idcompanyclient;
                objdet.Iddivision = infob.Iddivision;
                objdet.Idlocation = infob.Idlocation;
                objdet.Idrack = infob.Idrack;
                objdet.Level = infob.Level;
                objdet.Idbin = infob.Idbin;

                objlist.Add(objdet);

            }

            return ApiResponseFactory.Ok(objlist, "OK");
        }
    }
}
