using DUNES.API.Data;
using DUNES.Shared.DTOs.WMS;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.TransactionTypeClient
{
    /// <summary>
    /// Repository implementation for managing TransactionTypeClient mappings.
    /// Handles persistence and read operations for the mapping between a client
    /// and the master TransactionType catalog, scoped by CompanyId and CompanyClientId.
    /// </summary>
    public class TransactionTypeClientWMSAPIRepository : ITransactionTypeClientWMSAPIRepository
    {
        private readonly appWmsDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionTypeClientWMSAPIRepository"/> class.
        /// </summary>
        /// <param name="context">Application database context.</param>
        public TransactionTypeClientWMSAPIRepository(appWmsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all mapping records for the specified tenant scope (company + client),
        /// including both active and inactive mappings.
        /// </summary>
        public async Task<List<WMSTransactionTypeClientReadDTO>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            return await _context.TransactionTypeClients
                .Where(x =>
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId)
                .Select(x => new WMSTransactionTypeClientReadDTO
                {
                    Id = x.Id,
                    TransactionTypeId = x.TransactionTypeId,
                    TransactionTypeName = x.TransactionType.Name,
                    Active = x.Active
                })
                .ToListAsync(ct);
        }

        /// <summary>
        /// Gets a single mapping record by mapping Id within the specified tenant scope.
        /// </summary>
        public async Task<WMSTransactionTypeClientReadDTO?> GetByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            return await _context.TransactionTypeClients
                .Where(x =>
                    x.Id == id &&
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId)
                .Select(x => new WMSTransactionTypeClientReadDTO
                {
                    Id = x.Id,
                    TransactionTypeId = x.TransactionTypeId,
                    TransactionTypeName = x.TransactionType.Name,
                    Active = x.Active
                })
                .FirstOrDefaultAsync(ct);
        }

        /// <summary>
        /// Checks whether a mapping already exists for the same tenant scope and TransactionTypeId.
        /// Useful to prevent duplicates on create/update.
        /// </summary>
        public async Task<bool> ExistsMappingAsync(
            int companyId,
            int companyClientId,
            int transactionTypeId,
            int? excludeId,
            CancellationToken ct)
        {
            return await _context.TransactionTypeClients
                .AnyAsync(x =>
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId &&
                    x.TransactionTypeId == transactionTypeId &&
                    (!excludeId.HasValue || x.Id != excludeId.Value),
                    ct);
        }

        /// <summary>
        /// Checks whether the referenced TransactionType exists in the master catalog.
        /// </summary>
        public async Task<bool> MasterExistsAsync(
            int transactionTypeId,
            CancellationToken ct)
        {
            return await _context.Transactiontypes
                .AnyAsync(x => x.Id == transactionTypeId, ct);
        }

        /// <summary>
        /// Checks whether the referenced TransactionType exists and is active in the master catalog.
        /// This is used when enabling a mapping (Active = true).
        /// </summary>
        public async Task<bool> MasterIsActiveAsync(
            int transactionTypeId,
            CancellationToken ct)
        {
            return await _context.Transactiontypes
                .AnyAsync(x =>
                    x.Id == transactionTypeId &&
                    x.Active,
                    ct);
        }

        /// <summary>
        /// Gets the mapping entity by Id within the specified tenant scope.
        /// This method returns the entity (not DTO) for update/delete operations.
        /// </summary>
        public async Task<DUNES.API.ModelsWMS.Masters.TransactionTypeClient?> GetEntityByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            return await _context.TransactionTypeClients
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId,
                    ct);
        }

        /// <summary>
        /// Creates a new TransactionTypeClient mapping.
        /// </summary>
        public async Task<DUNES.API.ModelsWMS.Masters.TransactionTypeClient> CreateAsync(
            DUNES.API.ModelsWMS.Masters.TransactionTypeClient entity,
            CancellationToken ct)
        {
            _context.TransactionTypeClients.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        /// <summary>
        /// Updates an existing TransactionTypeClient mapping.
        /// </summary>
        public async Task<bool> UpdateAsync(
            DUNES.API.ModelsWMS.Masters.TransactionTypeClient entity,
            CancellationToken ct)
        {
            _context.TransactionTypeClients.Update(entity);
            return await _context.SaveChangesAsync(ct) > 0;
        }

        /// <summary>
        /// Deletes an existing TransactionTypeClient mapping.
        /// </summary>
        public async Task<bool> DeleteAsync(
            DUNES.API.ModelsWMS.Masters.TransactionTypeClient entity,
            CancellationToken ct)
        {
            _context.TransactionTypeClients.Remove(entity);
            return await _context.SaveChangesAsync(ct) > 0;
        }

        /// <summary>
        /// Sets the active status for an existing TransactionTypeClient mapping
        /// within the tenant scope.
        /// </summary>
        public async Task<bool> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            var entity = await _context.TransactionTypeClients
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId,
                    ct);

            if (entity == null)
                return false;

            entity.Active = isActive;

            return await _context.SaveChangesAsync(ct) > 0;
        }

        /// <summary>
        /// Returns the enabled transaction types for the current client.
        /// Only returns rows where:
        /// - Mapping Active = true AND
        /// - Master TransactionType Active = true
        /// </summary>
        public async Task<List<WMSTransactionTypeClientReadDTO>> GetEnabledAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            return await _context.TransactionTypeClients
                .Where(x =>
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId &&
                    x.Active &&
                    x.TransactionType.Active)
                .Select(x => new WMSTransactionTypeClientReadDTO
                {
                    Id = x.Id,
                    TransactionTypeId = x.TransactionTypeId,
                    TransactionTypeName = x.TransactionType.Name,
                    Active = x.Active
                })
                .ToListAsync(ct);
        }

        /// <summary>
        /// Deletes the transaction type relation by mapping Id
        /// (does not delete the master transaction type).
        /// </summary>
        public async Task<bool> DeleteAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct)
        {
            var entity = await _context.TransactionTypeClients
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId,
                    ct);

            if (entity == null)
                return false;

            _context.TransactionTypeClients.Remove(entity);
            return await _context.SaveChangesAsync(ct) > 0;
        }
    }
}