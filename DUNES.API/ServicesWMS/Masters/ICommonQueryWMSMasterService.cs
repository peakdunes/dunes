using DUNES.API.ModelsWMS.Masters;
using DUNES.API.ModelWMS.Masters;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;

namespace DUNES.API.ServicesWMS.Masters
{
    /// <summary>
    /// All common Master Tables queries
    /// </summary>
    public interface ICommonQueryWMSMasterService
    {

        /// <summary>
        /// Get all information for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<Company>> GetCompanyInformation(int companyid, CancellationToken ct);



        /// <summary>
        /// Get all Company Client actives for a Location id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="locationid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSLocationclientsDTO>>> GetAllActiveClientCompaniesByLocation(int companyid, int locationid, CancellationToken ct);

        /// <summary>
        /// Get all locations for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSLocationsDTO>>> GetAllActiveLocationsByCompany(int companyid, CancellationToken ct);

        /// <summary>
        /// Get all inventory type for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<InventoryTypes>>> GetAllInventoryTypesByCompanyClient(int companyid, string companyClient, CancellationToken ct);

        /// <summary>
        /// Get all actives inventory type for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<InventoryTypes>>> GetAllActiveInventoryTypesByCompanyClient(int companyid, string companyClient, CancellationToken ct);


        /// <summary>
        /// Get all item status for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<Itemstatus>>> GetAllItemStatusByCompanyClient(int companyid, string companyClient, CancellationToken ct);

        /// <summary>
        /// Get all actives inventory type for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<Itemstatus>>> GetAllActiveItemStatusByCompanyClient(int companyid, string companyClient, CancellationToken ct);


        /// <summary>
        /// Get all Rack for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<Racks>>> GetAllRacksByCompanyClient(int companyid, string companyClient, CancellationToken ct);

        /// <summary>
        /// Get all actives Racks for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<Racks>>> GetAllActiveRacksByCompanyClient(int companyid, string companyClient, CancellationToken ct);



        /// <summary>
        /// Get all Bins for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<Bines>>> GetAllBinsByCompanyClient(int companyid, string companyClient, CancellationToken ct);

        /// <summary>
        /// Get all actives Bins for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<Bines>>> GetAllActiveBinsByCompanyClient(int companyid, string companyClient, CancellationToken ct);


        /// <summary>
        /// Get all Warehouse Organization for a Client Company
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSWarehouseorganizationDto>>> GetAllWareHouseOrganizationByCompanyClient(int companyid, string companyClient, CancellationToken ct);


    }
}
