using DUNES.API.ReadModels.Inventory;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace DUNES.UI.Services.Inventory.PickProcess
{
    public interface IPickProcessService
    {
      
        Task<ApiResponse<PickProcessRequestDto>>  GetPickProcessAllInfo(string DeliveryId, string token, CancellationToken ct);


        Task<ApiResponse<PickProcessCallsReadDto>> GetPickProcessAllCalls(string DeliveryId, string token, CancellationToken ct);


        Task<ApiResponse<WMSTransactionTm>> GetAllTransactionByDocumentNumber(int companyid, string companyClient, string DocumentNumber, string token, CancellationToken ct);


        Task<ApiResponse<TorderRepairTm>> GetAllTablesOrderRepairCreatedByPickProcessAsync(string ConsignRequestId, string token, CancellationToken ct);
    }


}
