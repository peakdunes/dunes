using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.WiewModels.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.UI.Services.Inventory
{
    public interface IASNService
    {

        Task<ApiResponse<ASNWm>> GetAsnInfo(string asnNumber, string token,CancellationToken ct);

        Task<ApiResponse<List<WMSClientCompaniesDto>>> GetClientCompanies(string token, CancellationToken ct);

        Task<ApiResponse<List<WMSBinsDto>>> GetAllActiveBinsByCompanyClient (int companyid, string companyClient, string token, CancellationToken ct);

        Task<ApiResponse<List<WMSConceptsDto>>> GetAllActiveConceptsByCompanyClient(int companyid, string companyClient, string token, CancellationToken ct);

        Task<ApiResponse<List<WMSInputTransactionsDto>>> GetAllActiveInputTransactionsByCompanyClient(int companyid, string companyClient, string token, CancellationToken ct);

        Task<ApiResponse<List<itemstatusDto>>> GetAllActiveItemStatus(int companyid, string companyClient, string token, CancellationToken ct);

        Task<ApiResponse<List<InventoryTypeDto>>> GetAllActiveInventoryTypes(string token, CancellationToken ct);


        Task<ApiResponse<List<WMSInventoryTypeDto>>> GetAllActiveWmsInventoryTypes(int companyid, string companyClient, string token, CancellationToken ct);


    }
}
