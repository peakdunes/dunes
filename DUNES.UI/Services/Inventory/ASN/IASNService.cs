using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.WiewModels.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.UI.Services.Inventory.ASN
{
    public interface IASNService
    {
        /// <summary>
        /// Get all information (Header and Items Detail for an ASN)
        /// </summary>
        /// <param name="asnNumber"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<ASNWm>> GetAsnInfo(string asnNumber, string token,CancellationToken ct);

        /// <summary>
        /// Get all company client information created in WMS system
        /// </summary>
        /// <param name="asnNumber"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSClientCompaniesDto>>> GetClientCompanies(string token, CancellationToken ct);
        /// <summary>
        /// All active Bins for a company client
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSBinsDto>>> GetAllActiveBinsByCompanyClient (int companyid, string companyClient, string token, CancellationToken ct);

        /// <summary>
        /// All active Transaction Inventory Concepts for a company client
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSConceptsDto>>> GetAllActiveConceptsByCompanyClient(int companyid, string companyClient, string token, CancellationToken ct);


        /// <summary>
        /// All active input Transaction Inventory for a company client
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSInputTransactionsDto>>> GetAllActiveInputTransactionsByCompanyClient(int companyid, string companyClient, string token, CancellationToken ct);


        /// <summary>
        /// All active output Transaction Inventory for a company client
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSInputTransactionsDto>>> GetAllActiveOutputTransactionsByCompanyClient(int companyid, string companyClient, string token, CancellationToken ct);


        /// <summary>
        /// All active item status for a company client
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<itemstatusDto>>> GetAllActiveItemStatus(int companyid, string companyClient, string token, CancellationToken ct);

        /// <summary>
        /// All active inventory ZEBRA types for a company client
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<InventoryTypeDto>>> GetAllActiveInventoryTypes(string token, CancellationToken ct);


        /// <summary>
        /// All active inventory WMS types for a company client
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSInventoryTypeDto>>> GetAllActiveWmsInventoryTypes(int companyid, string companyClient, string token, CancellationToken ct);


        /// <summary>
        /// Get All Item inventory for a item company client
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>

        Task<ApiResponse<List<WMSInventoryDetailByPartNumberDto>>> GetInventoryByItem(int companyid, string companyClient, string parnumber, string token, CancellationToken ct);


        /// <summary>
        /// All Item Bin distribution for a item company client
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// 

        Task<ApiResponse<List<WMSItemByBinsDto>>> GetItemBinsDistribution(int companyid, string companyClient, string parnumber, string token, CancellationToken ct);

    }
}
