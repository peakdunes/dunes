using DUNES.API.Models.Inventory;
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
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<TzebB2bReplacementPartsInventoryLogDto>>> GetAllInventoryTransactionsByDocument(string DocumentNumber, CancellationToken ct)
        {
           
            var infotransactions = await _repository.GetAllInventoryTransactionsByDocument(DocumentNumber);

            if (infotransactions == null)
            {
                return ApiResponseFactory.NotFound<List<TzebB2bReplacementPartsInventoryLogDto>>(
                   $"There is not transactions for this document ({DocumentNumber}).");
            }

            return ApiResponseFactory.Ok(infotransactions!, "OK");
        }
        /// <summary>
        /// Get all inventory transactions for a Part Number ID
        /// </summary>
        /// <param name="PartNumberId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<TzebB2bReplacementPartsInventoryLogDto>>> GetAllInventoryTransactionsByPartNumberId(int PartNumberId, CancellationToken ct)
        {
            var infotransactions = await _repository.GetAllInventoryTransactionsByPartNumberId(PartNumberId);

            if (infotransactions == null)
            {
                return ApiResponseFactory.NotFound<List<TzebB2bReplacementPartsInventoryLogDto>>(
                   $"There is not transactions for this Part Number Id ({PartNumberId}).");
            }

            return ApiResponseFactory.Ok(infotransactions!, "OK");
        }

        /// <summary>
        /// Get all Division for a company
        /// </summary>
        /// <param name="CompanyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<TdivisionCompanyDto>>> GetDivisionByCompanyClient(string CompanyClient, CancellationToken ct)
        {
            var infodivisions = await _repository.GetDivisionByCompanyClient(CompanyClient);

            if (infodivisions == null)
            {
                return ApiResponseFactory.NotFound<List<TdivisionCompanyDto>>(
                   $"There is not division for this company ({CompanyClient}).");
            }

            List<TdivisionCompanyDto> listdiv = new List<TdivisionCompanyDto>();
            TdivisionCompanyDto objdet = new TdivisionCompanyDto();

            foreach (var company in infodivisions)
            {
                objdet.CompanyDsc = company.CompanyDsc;
                objdet.DivisionDsc = company.DivisionDsc;

                listdiv.Add(objdet);
            }

            return ApiResponseFactory.Ok(listdiv!, "OK");
        }
    }
}
