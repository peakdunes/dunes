using DUNES.API.ReadModels.Inventory;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.DTOs.Masters;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using DUNES.Shared.WiewModels.Inventory;
using System.Runtime.CompilerServices;

namespace DUNES.UI.Services.Inventory.Common
{
    public interface ICommonINVUIService
    {


        /// <summary>
        /// Get all active location for a company
        /// </summary>
        /// <param name="asnNumber"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSLocationsUpdateDTO>>> GetAllActiveLocationsByCompany(int companyid, string token, CancellationToken ct);




        /// <summary>
        /// Get all company client information created in WMS system
        /// </summary>
        /// <param name="asnNumber"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSLocationclientsDTO>>> GetAllActiveClientCompaniesByLocation( int companyid, int locationid, string token, CancellationToken ct);

       


        /// <summary>
        /// Get all company client information created in WMS system
        /// </summary>
        /// <param name="asnNumber"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WmsCompanyclientDto>> GetClientCompanyInformationByName(string companyname,  string token, CancellationToken ct);

       

        /// <summary>
        /// Get all division by company client
        /// </summary>
        /// <param name="companyclient"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<TdivisionCompanyDto>>> GetDivisionByCompanyClient(string companyclient ,string token, CancellationToken ct);


        /// <summary>
        /// All active Bins for a company client
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSBinsCreateDto>>> GetAllActiveBinsByCompanyClient(int companyid, string companyClient, string token, CancellationToken ct);

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
        Task<ApiResponse<List<WMSTransactionsDto>>> GetAllActiveInputTransactionsByCompanyClient(int companyid, string companyClient, string token, CancellationToken ct);


        /// <summary>
        /// All active output Transaction Inventory for a company client
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSTransactionsDto>>> GetAllActiveOutputTransactionsByCompanyClient(int companyid, string companyClient, string token, CancellationToken ct);


        /// <summary>
        /// Get All Active Input Type Transfer transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task<ApiResponse<List<WMSTransactionsDto>>> GetAllActiveTransferTransactionsInputType(int companyid, string companyClient, string token, CancellationToken ct);


        /// <summary>
        /// Get All Input Type Transfer transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task<ApiResponse<List<WMSTransactionsDto>>> GetAllTransferTransactionsInputType(int companyid, string companyClient, string token, CancellationToken ct);



        /// <summary>
        /// Get All Active Output Type Transfer transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task<ApiResponse<List<WMSTransactionsDto>>> GetAllActiveTransferTransactionsOutputType(int companyid, string companyClient, string token, CancellationToken ct);

        /// <summary>
        /// Get All Output Type Transfer transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task<ApiResponse<List<WMSTransactionsDto>>> GetAllTransferTransactionsOutputType(int companyid, string companyClient, string token, CancellationToken ct);


        /// <summary>
        /// Get one transaction type by ID
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task<ApiResponse<WMSTransactionsDto>> GetTransactionsTypeById(int companyid, string companyClient, int id, string token, CancellationToken ct);



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
        /// Get All Item inventory by Inventory Type  for a item company client
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="parnumber"></param>
        /// <param name="typeid"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSInventoryDetailByPartNumberDto>>> GetInventoryByItemInventoryType(int companyid, string companyClient, string parnumber, int typeid, string token,  CancellationToken ct);
                

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


        /// <summary>
        /// Get all Warehouse Organization for a Client Company
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSWarehouseorganizationDto>>> GetAllWareHouseOrganizationByCompanyClient(int companyid, string companyClient, string token, CancellationToken ct);


        /// <summary>
        /// Get all information about an item by partnumber
        /// </summary>
        /// <param name="partnumber"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<TzebB2bMasterPartDefinitionDto>> GetByPartNumber(string partnumber, string token, CancellationToken ct);



        /// <summary>
        /// Get all inventory transactions ZEBRA Database for a Document Number and a search Start Date
        /// </summary>
        /// <param name="DocumentNumber"></param>
        /// <param name="startDate"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<TzebB2bReplacementPartsInventoryLogDto>>> GetAllInventoryTransactionsByDocumentStartDate(string DocumentNumber, DateTime startDate, string token, CancellationToken ct);



        /// <summary>
        /// Get all input - output call for a ASN - Pick Process Document
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<PickProcessCallsReadDto>> GetAllCalls(string DocumentId, string processtype, string token, CancellationToken ct);


        /// <summary>
        /// Get all WMS Transaction for a document
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="DocumentNumber"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSTransactionTm>> GetAllWMSTransactionByDocumentNumber(int companyid, string companyClient, string DocumentNumber, string token, CancellationToken ct);
    }
}
