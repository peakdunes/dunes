using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;
using DUNES.UI.Services.WMS.Masters.TransactionConcepts;

namespace DUNES.UI.Services.WMS.Masters.TransactionTypes
{
    public class TransactionTypesWMSUIService : UIApiServiceBase, ITransactionTypesWMSUIService
    {


        /// <summary>
        /// Base API route for Inventory Categories controller.
        /// </summary>
        private const string BasePath = "/api/wms/masters/transaction-types";

        /// <summary>
        /// Initializes a new instance of <see cref="InventoryCategoriesWMSUIService"/>.
        /// </summary>
        /// <param name="factory">HTTP client factory.</param>
        public TransactionTypesWMSUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        public Task<ApiResponse<bool>> CreateAsync(WMSTransactiontypesCreateDTO entity, string token, CancellationToken ct)
        => PostApiAsync<bool, WMSTransactiontypesCreateDTO>(
                $"{BasePath}/Create",
                entity,
                token,
                ct);

        public Task<ApiResponse<bool>> DeleteByIdAsync(string token, int id, CancellationToken ct)
        => DeleteApiAsync<bool>(
                $"{BasePath}/Delete/{id}",
                token,
                ct);

        public Task<ApiResponse<bool>> ExistsByNameAsync(string name, int? excludeId, string token, CancellationToken ct)
        => GetApiAsync<bool>(
                $"{BasePath}/ExistsByName?name={Uri.EscapeDataString(name)}&excludeId={excludeId}",
                token,
                ct);

        public Task<ApiResponse<List<WMSTransactiontypesReadDTO>>> GetAllAsync(string token, CancellationToken ct)

            => GetApiAsync<List<WMSTransactiontypesReadDTO>>(
                $"{BasePath}/GetAll",
                token,
                ct);


        public Task<ApiResponse<WMSTransactiontypesReadDTO?>> GetByIdAsync(int id, string token, CancellationToken ct)
        => GetApiAsync<WMSTransactiontypesReadDTO>(
                $"{BasePath}/GetById/{id}",
                token,
                ct);

        public Task<ApiResponse<bool>> SetActiveAsync(int id, bool isActive, string token, CancellationToken ct)
        => PatchApiAsync<bool>(
                $"{BasePath}/SetActive/{id}?isActive={isActive.ToString().ToLowerInvariant()}",
                token: token,
                ct: ct);

        public Task<ApiResponse<bool>> UpdateAsync(int id, WMSTransactionTypesUpdateDTO entity, string token, CancellationToken ct)
          => PutApiAsync<bool, WMSTransactionTypesUpdateDTO>(
                $"{BasePath}/Update/{id}",
                entity,
                token,
                ct);
    }
}
