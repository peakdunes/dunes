using DUNES.API.ReadModels.Inventory;
using DUNES.API.Repositories.Inventory.Common.Queries;
using DUNES.API.Utils.Responses;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;

namespace DUNES.API.Services.Inventory.Common.Queries
{
    /// <summary>
    /// All common inventory transactions queries
    /// </summary>
    public class CommonQueryINVService : ICommonQueryINVService
    {

        private readonly ICommonQueryINVRepository _repository;

        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="repository"></param>
        public CommonQueryINVService(ICommonQueryINVRepository repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// get all inventory transactions for a document number
        /// </summary>
        /// <param name="DocumentNumber"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<TzebB2bReplacementPartsInventoryLogDto>>> GetAllInventoryTransactionsByDocument(string DocumentNumber)
        {
           
            var infotransactions = await _repository.GetAllInventoryTransactionsByDocument(DocumentNumber);

            if (infotransactions == null)
            {
                return ApiResponseFactory.NotFound<List<TzebB2bReplacementPartsInventoryLogDto>>(
                   $"There is not transactions for this document ({DocumentNumber}).");
            }

            return ApiResponseFactory.Ok(infotransactions!, "OK");
        }
    }
}
