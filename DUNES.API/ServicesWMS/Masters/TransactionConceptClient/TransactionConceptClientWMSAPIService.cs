using DUNES.API.Data;
using DUNES.API.RepositoriesWMS.Masters.ClientCompanies;
using DUNES.API.RepositoriesWMS.Masters.Companies;
using DUNES.API.RepositoriesWMS.Masters.TransactionConceptClient;
using DUNES.API.RepositoriesWMS.Masters.TransactionConcepts;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.ServicesWMS.Masters.TransactionConceptClient
{
    /// <summary>
    /// Transaction Concept Client service implementation.
    ///
    /// Enforces business rules for mapping Transaction Concepts to Company Clients.
    ///
    /// IMPORTANT (STANDARD COMPANYID):
    /// - CompanyId is always provided by Controller (from token).
    /// - Service enforces tenant/client ownership rules.
    /// - Repository does persistence only.
    /// </summary>
    public class TransactionConceptClientWMSAPIService : ITransactionConceptClientWMSAPIService
    {
        private readonly ITransactionConceptClientWMSAPIRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionConceptClientWMSAPIService"/> class.
        /// </summary>
        /// <param name="repository">Repository dependency.</param>
        public TransactionConceptClientWMSAPIService(
            ITransactionConceptClientWMSAPIRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves all mappings for a specific company client.
        /// </summary>
        public async Task<ApiResponse<List<WMSTransactionConceptClientReadDTO>>> GetByClientAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            // Validar que el client pertenezca a la compañía (premisa multi-tenant)
            var clientExists = await _repository.CompanyClientExistsAsync(companyId, companyClientId, ct);
            if (!clientExists)
            {
                return ApiResponseFactory.NotFound<List<WMSTransactionConceptClientReadDTO>>(
                    "Company client not found for this company.");
            }

            var data = await _repository.GetByClientAsync(companyId, companyClientId, ct);
            return ApiResponseFactory.Success(data);
        }

        /// <summary>
        /// Creates a new mapping between CompanyClient and TransactionConcept.
        /// </summary>
        public async Task<ApiResponse<WMSTransactionConceptClientReadDTO>> CreateAsync(
            int companyId,
            WMSTransactionConceptClientCreateDTO dto,
            CancellationToken ct)
        {
            // 1) Validar client scope
            var clientExists = await _repository.CompanyClientExistsAsync(companyId, dto.CompanyClientId, ct);
            if (!clientExists)
            {
                return ApiResponseFactory.NotFound<WMSTransactionConceptClientReadDTO>(
                    "Company client not found for this company.");
            }

            // 2) Validar master
            var masterExists = await _repository.MasterExistsAsync(companyId, dto.TransactionConceptId, ct);
            if (!masterExists)
            {
                return ApiResponseFactory.NotFound<WMSTransactionConceptClientReadDTO>(
                    "Transaction concept master not found for this company.");
            }

            // 3) Validar duplicado
            var exists = await _repository.ExistsAsync(
                companyId,
                dto.CompanyClientId,
                dto.TransactionConceptId,
                excludeId: null,
                ct);

            if (exists)
            {
                return ApiResponseFactory.Fail<WMSTransactionConceptClientReadDTO>(
                    error: "DUPLICATE_MAPPING",
                    message: "This transaction concept is already associated with the company client.",
                    statusCode: StatusCodes.Status409Conflict);
            }

            // 4) Crear entidad
            var entity = new ModelsWMS.Masters.TransactionConceptClient
            {
                CompanyId = companyId, // SIEMPRE desde token
                CompanyClientId = dto.CompanyClientId,
                TransactionConceptId = dto.TransactionConceptId,
                Active = dto.Active
            };

            var created = await _repository.CreateAsync(entity, ct);

            // 5) Re-leer para devolver DTO completo con nombre del master
            var rows = await _repository.GetByClientAsync(companyId, dto.CompanyClientId, ct);
            var createdDto = rows.FirstOrDefault(x => x.Id == created.Id);

            if (createdDto is null)
            {
                // Esto no debería pasar, pero mejor respuesta consistente que explotar
                return ApiResponseFactory.Fail<WMSTransactionConceptClientReadDTO>(
                    error: "CREATE_READBACK_FAILED",
                    message: "Mapping was created but could not be loaded for response.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }

            return ApiResponseFactory.Success(createdDto);
        }

        /// <summary>
        /// Activates or deactivates a mapping (patch style).
        /// </summary>
        public async Task<ApiResponse<bool>> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            // Validar client scope
            var clientExists = await _repository.CompanyClientExistsAsync(companyId, companyClientId, ct);
            if (!clientExists)
            {
                return ApiResponseFactory.NotFound<bool>(
                    "Company client not found for this company.");
            }

            // Validar mapping (opcional pero útil para mensajes claros)
            var entity = await _repository.GetEntityByIdAsync(companyId, companyClientId, id, ct);
            if (entity is null)
            {
                return ApiResponseFactory.NotFound<bool>(
                    "Transaction concept client mapping not found.");
            }

            var ok = await _repository.SetActiveAsync(companyId, companyClientId, id, isActive, ct);

            if (!ok)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "PATCH_FAILED",
                    message: "The mapping could not be updated.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }

            return ApiResponseFactory.Success(true);
        }

        /// <summary>
        /// Deletes a mapping physically.
        /// </summary>
        public async Task<ApiResponse<bool>> DeleteAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct)
        {
            // Validar client scope
            var clientExists = await _repository.CompanyClientExistsAsync(companyId, companyClientId, ct);
            if (!clientExists)
            {
                return ApiResponseFactory.NotFound<bool>(
                    "Company client not found for this company.");
            }

            var entity = await _repository.GetEntityByIdAsync(companyId, companyClientId, id, ct);
            if (entity is null)
            {
                return ApiResponseFactory.NotFound<bool>(
                    "Transaction concept client mapping not found.");
            }

            var deleted = await _repository.DeleteAsync(companyId, companyClientId, id, ct);

            if (!deleted)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "DELETE_FAILED",
                    message: "The mapping could not be deleted.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }

            return ApiResponseFactory.Success(true);
        }
    }
}
