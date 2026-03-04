using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.WMS.Masters.TransactionConcepts
{
    public class TransactionConceptsWMSUIService : UIApiServiceBase, ITransactionConceptsWMSUIService
    {

        /// <summary>
        /// Base API route for Inventory Categories controller.
        /// </summary>
        private const string BasePath = "/api/wms/masters/transaction-concepts";

        /// <summary>
        /// Initializes a new instance of <see cref="InventoryCategoriesWMSUIService"/>.
        /// </summary>
        /// <param name="factory">HTTP client factory.</param>
        public TransactionConceptsWMSUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        public Task<ApiResponse<bool>> CreateAsync(WMSTransactionconceptsCreateDTO entity, string token, CancellationToken ct)
        => PostApiAsync<bool, WMSTransactionconceptsCreateDTO>(
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

        public Task<ApiResponse<List<WMSTransactionconceptsReadDTO>>> GetAllAsync(string token, CancellationToken ct)
       
            => GetApiAsync<List<WMSTransactionconceptsReadDTO>>(
                $"{BasePath}/GetAll",
                token,
                ct);
       

        public Task<ApiResponse<WMSTransactionconceptsReadDTO?>> GetByIdAsync(int id, string token, CancellationToken ct)
        => GetApiAsync<WMSTransactionconceptsReadDTO>(
                $"{BasePath}/GetById/{id}",
                token,
                ct);

        public Task<ApiResponse<bool>> SetActiveAsync(int id, bool isActive, string token, CancellationToken ct)
        => PatchApiAsync<bool>(
                $"{BasePath}/SetActive/{id}?isActive={isActive.ToString().ToLowerInvariant()}",
                token: token,
                ct: ct);

        public Task<ApiResponse<bool>> UpdateAsync(int id, WMSTransactionconceptsUpdateDTO entity, string token, CancellationToken ct)
          => PutApiAsync<bool, WMSTransactionconceptsUpdateDTO>(
                $"{BasePath}/Update/{id}",
                entity,
                token,
                ct);
    }
}
