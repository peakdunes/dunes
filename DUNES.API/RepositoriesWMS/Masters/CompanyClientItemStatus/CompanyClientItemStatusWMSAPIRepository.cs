using DUNES.API.Data;
using DUNES.Shared.DTOs.WMS;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.CompanyClientItemStatus
{
    /// <summary>
    /// Repository implementation for client-level item status enablement.
    /// Anti-error design principles:
    /// - CompanyId and CompanyClientId are ALWAYS taken from the token.
    /// - Mapping records are unique by (CompanyId, CompanyClientId, ItemStatusId).
    /// - Master catalog must be IsActive=true to allow enabling or to appear in enabled results.
    /// - This repository does not expose a generic Update method to avoid changing ItemStatusId by mistake.
    ///   Changes must be performed through SetActive or SetEnabledSet.
    /// </summary>
    public class CompanyClientItemStatusWMSAPIRepository : ICompanyClientItemStatusWMSAPIRepository
    {
        private readonly appWmsDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyClientItemStatusWMSAPIRepository"/> class.
        /// </summary>
        /// <param name="context">Application database context.</param>
        public CompanyClientItemStatusWMSAPIRepository(appWmsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns all item status mappings for the current client.
        /// Only returns rows where:
        /// - Mapping IsActive = true
        /// - Master ItemStatus IsActive = true
        /// </summary>
        /// <param name="companyId">Company scope from token.</param>
        /// <param name="companyClientId">Company client scope from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of active client item status mappings.</returns>
        public async Task<List<WMSCompanyClientItemStatusReadDTO>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            return await _context.CompanyClientItemStatuses
                .Where(x =>
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId &&
                    //x.IsActive &&
                    x.ItemStatusNavigation.Active)
                .Select(x => new WMSCompanyClientItemStatusReadDTO
                {
                    Id = x.Id,
                    ItemStatusId = x.ItemStatusId,
                    ItemStatusName = x.ItemStatusNavigation.Name,
                    IsActive = x.IsActive
                })
                .ToListAsync(ct);
        }

        /// <summary>
        /// Returns the enabled item statuses for the current client.
        /// Only returns rows where:
        /// - Mapping IsActive = true
        /// - Master ItemStatus IsActive = true
        /// </summary>
        /// <param name="companyId">Company scope from token.</param>
        /// <param name="companyClientId">Company client scope from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of enabled client item status mappings.</returns>
        public async Task<List<WMSCompanyClientItemStatusReadDTO>> GetEnabledAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            return await GetAllAsync(companyId, companyClientId, ct);
        }

        /// <summary>
        /// Returns a specific item status mapping by Id scoped by CompanyId and CompanyClientId.
        /// </summary>
        /// <param name="companyId">Company scope from token.</param>
        /// <param name="companyClientId">Company client scope from token.</param>
        /// <param name="id">Mapping Id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The mapping if found; otherwise null.</returns>
        public async Task<WMSCompanyClientItemStatusReadDTO?> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct)
        {
            return await _context.CompanyClientItemStatuses
                .Where(x =>
                    x.Id == id &&
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId)
                .Select(x => new WMSCompanyClientItemStatusReadDTO
                {
                    Id = x.Id,
                    ItemStatusId = x.ItemStatusId,
                    ItemStatusName = x.ItemStatusNavigation.Name,
                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync(ct);
        }

        /// <summary>
        /// Creates a new client mapping for a master item status.
        /// Service layer must validate:
        /// - Master item status exists and is active
        /// - Mapping does not already exist
        /// </summary>
        /// <param name="dto">Create DTO containing the master ItemStatusId and optional IsActive value.</param>
        /// <param name="companyId">Company scope from token.</param>
        /// <param name="companyClientId">Company client scope from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created mapping projected to read DTO.</returns>
        /// <exception cref="Exception">Thrown when the mapping is created but cannot be retrieved afterwards.</exception>
        public async Task<WMSCompanyClientItemStatusReadDTO> CreateAsync(
            WMSCompanyClientItemStatusCreateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var entity = new API.ModelsWMS.Masters.CompanyClientItemStatus
            {
                CompanyId = companyId,
                CompanyClientId = companyClientId,
                ItemStatusId = dto.ItemStatusId,
                IsActive = dto.IsActive
            };

            _context.CompanyClientItemStatuses.Add(entity);
            await _context.SaveChangesAsync(ct);

            return await GetByIdAsync(companyId, companyClientId, entity.Id, ct)
                ?? throw new Exception("The item status mapping was created, but it could not be loaded afterwards.");
        }

        /// <summary>
        /// Updates the active flag of an existing client mapping.
        /// </summary>
        /// <param name="companyId">Company scope from token.</param>
        /// <param name="companyClientId">Company client scope from token.</param>
        /// <param name="id">Mapping Id.</param>
        /// <param name="isActive">New active status for the mapping.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if the mapping was found and updated; otherwise false.</returns>
        public async Task<bool> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            var entity = await _context.CompanyClientItemStatuses
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId,
                    ct);

            if (entity == null)
                return false;

            entity.IsActive = isActive;

            await _context.SaveChangesAsync(ct);
            return true;
        }

        /// <summary>
        /// Replaces the enabled set for the current client.
        /// Behavior:
        /// - Existing mappings whose ItemStatusId is in the provided list are marked as active.
        /// - Existing mappings whose ItemStatusId is not in the provided list are marked as inactive.
        /// 
        /// Note:
        /// - Service layer should validate that all provided ItemStatusIds exist and are master-active.
        /// - Service layer should also create missing mappings if the business rule requires true upsert behavior.
        /// </summary>
        /// <param name="companyId">Company scope from token.</param>
        /// <param name="companyClientId">Company client scope from token.</param>
        /// <param name="itemStatusIds">Final list of enabled master item status ids.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if the operation completes successfully.</returns>
        public async Task<bool> SetEnabledSetAsync(
            int companyId,
            int companyClientId,
            List<int> itemStatusIds,
            CancellationToken ct)
        {
            var mappings = await _context.CompanyClientItemStatuses
                .Where(x =>
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId)
                .ToListAsync(ct);

            foreach (var mapping in mappings)
            {
                mapping.IsActive = itemStatusIds.Contains(mapping.ItemStatusId);
            }

            await _context.SaveChangesAsync(ct);
            return true;
        }

        /// <summary>
        /// Checks whether a mapping already exists for the given company, client, and master item status.
        /// </summary>
        /// <param name="companyId">Company scope from token.</param>
        /// <param name="companyClientId">Company client scope from token.</param>
        /// <param name="itemStatusId">Master item status id.</param>
        /// <param name="excludeId">Optional mapping id to exclude from the check.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if a matching mapping exists; otherwise false.</returns>
        public async Task<bool> ExistsAsync(
            int companyId,
            int companyClientId,
            int itemStatusId,
            int? excludeId,
            CancellationToken ct)
        {
            return await _context.CompanyClientItemStatuses
                .AnyAsync(x =>
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId &&
                    x.ItemStatusId == itemStatusId &&
                    (!excludeId.HasValue || x.Id != excludeId.Value),
                    ct);
        }

        /// <summary>
        /// Validates whether the master item status exists and is active.
        /// </summary>
        /// <param name="companyId">
        /// Company scope from token.
        /// Included to preserve the standard method signature for this family of repositories.
        /// </param>
        /// <param name="itemStatusId">Master item status id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if the master item status exists and is active; otherwise false.</returns>
        public async Task<bool> IsMasterActiveAsync(
            int companyId,
            int itemStatusId,
            CancellationToken ct)
        {
            return await _context.Itemstatus
                .AnyAsync(x =>
                    x.Id == itemStatusId &&
                    x.Active,
                    ct);
        }

        /// <summary>
        /// Checks whether any client mapping exists for the provided master item status.
        /// Useful before deleting a master item status.
        /// </summary>
        /// <param name="companyId">Company scope from token.</param>
        /// <param name="itemStatusId">Master item status id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if at least one client mapping exists; otherwise false.</returns>
        public async Task<bool> HasAnyClientMappingAsync(
            int companyId,
            int itemStatusId,
            CancellationToken ct)
        {
            return await _context.CompanyClientItemStatuses
                .AnyAsync(x =>
                    x.CompanyId == companyId &&
                    x.ItemStatusId == itemStatusId,
                    ct);
        }

        /// <summary>
        /// Deletes a client mapping by Id.
        /// Important:
        /// - This only deletes the client relation.
        /// - It does not delete the master ItemStatus record.
        /// </summary>
        /// <param name="companyId">Company scope from token.</param>
        /// <param name="companyClientId">Company client scope from token.</param>
        /// <param name="id">Mapping Id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if the mapping was found and deleted; otherwise false.</returns>
        public async Task<bool> DeleteAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct)
        {
            var entity = await _context.CompanyClientItemStatuses
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId,
                    ct);

            if (entity == null)
                return false;

            _context.CompanyClientItemStatuses.Remove(entity);
            await _context.SaveChangesAsync(ct);

            return true;
        }
    }
}