using DUNES.API.Data;
using DUNES.Shared.DTOs.WMS;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.CompanyClientInventoryType
{

    /// <summary>
    /// Repository implementation for managing inventory type enablement per client.
    /// Strictly scoped by CompanyId + CompanyClientId (from token).
    /// </summary>
    public class CompanyClientInventoryTypeWMSAPIRepository : ICompanyClientInventoryTypeWMSAPIRepository
    {
        private readonly appWmsDbContext _db;


        /// <summary>
        /// Constructor (DI)
        /// </summary>
        /// <param name="db"></param>
        public CompanyClientInventoryTypeWMSAPIRepository(appWmsDbContext db)
        {
            _db = db;
        }

        /// <inheritdoc/>
        public async Task<List<WMSCompanyClientInventoryTypeReadDTO>> GetEnabledAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            // Enabled means: mapping IsActive=true AND master IsActive=true
            return await _db.CompanyClientInventoryTypes
                .AsNoTracking()
                .Where(m => m.CompanyId == companyId
                         && m.CompanyClientId == companyClientId
                         && m.IsActive)
                .Join(
                    _db.InventoryTypes.AsNoTracking().Where(t => t.Active),
                    m => m.InventoryTypeId,
                    t => t.Id,
                    (m, t) => new WMSCompanyClientInventoryTypeReadDTO
                    {
                        Id = m.Id,
                        InventoryTypeId = t.Id,
                        InventoryTypeName = t.Name,
                        IsActive = m.IsActive
                    })
                .OrderBy(x => x.InventoryTypeName)
                .ToListAsync(ct);
        }

        /// <inheritdoc/>
        public async Task<WMSCompanyClientInventoryTypeReadDTO?> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct)
        {
            // Consistent with GetEnabledAsync: require master active.
            return await _db.CompanyClientInventoryTypes
                .AsNoTracking()
                .Where(m => m.Id == id
                         && m.CompanyId == companyId
                         && m.CompanyClientId == companyClientId)
                .Join(
                    _db.InventoryTypes.AsNoTracking().Where(t => t.Active),
                    m => m.InventoryTypeId,
                    t => t.Id,
                    (m, t) => new WMSCompanyClientInventoryTypeReadDTO
                    {
                        Id = m.Id,
                        InventoryTypeId = t.Id,
                        InventoryTypeName = t.Name,
                        IsActive = m.IsActive
                    })
                .FirstOrDefaultAsync(ct);
        }

        /// <inheritdoc/>
        public async Task<WMSCompanyClientInventoryTypeReadDTO> CreateAsync(
            WMSCompanyClientInventoryTypeCreateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            // Anti-error: master must be active
            var master = await _db.InventoryTypes
                .AsNoTracking()
                .Where(t => t.Id == dto.InventoryTypeId && t.Active)
                .Select(t => new { t.Id, t.Name })
                .FirstOrDefaultAsync(ct);

            if (master is null)
                throw new InvalidOperationException("Cannot create mapping: master inventory type is invalid or inactive.");

            // Anti-error: avoid duplicates (unique index should also exist)
            var exists = await _db.CompanyClientInventoryTypes
                .AsNoTracking()
                .AnyAsync(m =>
                    m.CompanyId == companyId &&
                    m.CompanyClientId == companyClientId &&
                    m.InventoryTypeId == dto.InventoryTypeId,
                    ct);

            if (exists)
                throw new InvalidOperationException("Mapping already exists for this client and inventory type.");

            var entity = new ModelsWMS.Masters.CompanyClientInventoryType
            {
                CompanyId = companyId,
                CompanyClientId = companyClientId,
                InventoryTypeId = dto.InventoryTypeId,
                IsActive = dto.IsActive
            };

            _db.CompanyClientInventoryTypes.Add(entity);
            await _db.SaveChangesAsync(ct);

            return new WMSCompanyClientInventoryTypeReadDTO
            {
                Id = entity.Id,
                InventoryTypeId = master.Id,
                InventoryTypeName = master.Name,
                IsActive = entity.IsActive
            };
        }

        /// <inheritdoc/>
        public async Task<bool> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            var entity = await _db.CompanyClientInventoryTypes
                .FirstOrDefaultAsync(m =>
                    m.Id == id &&
                    m.CompanyId == companyId &&
                    m.CompanyClientId == companyClientId,
                    ct);

            if (entity is null)
                return false;

            // Anti-error: do not allow activation if master is inactive
            if (isActive)
            {
                var masterActive = await IsMasterActiveAsync(companyId, entity.InventoryTypeId, ct);
                if (!masterActive)
                    throw new InvalidOperationException("Cannot activate mapping: master inventory type is inactive.");
            }

            entity.IsActive = isActive;
            await _db.SaveChangesAsync(ct);
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> SetEnabledSetAsync(
            int companyId,
            int companyClientId,
            List<int> inventoryTypeIds,
            CancellationToken ct)
        {
            inventoryTypeIds ??= new List<int>();
            inventoryTypeIds = inventoryTypeIds.Where(x => x > 0).Distinct().ToList();

            // Validate all provided IDs are master-active
            if (inventoryTypeIds.Count > 0)
            {
                var activeIds = await _db.InventoryTypes
                    .AsNoTracking()
                    .Where(t => t.Active && inventoryTypeIds.Contains(t.Id))
                    .Select(t => t.Id)
                    .ToListAsync(ct);

                var missingOrInactive = inventoryTypeIds.Except(activeIds).ToList();
                if (missingOrInactive.Count > 0)
                    throw new InvalidOperationException($"Invalid/inactive master inventory types: {string.Join(",", missingOrInactive)}");
            }

            // Load current mappings for the client
            var current = await _db.CompanyClientInventoryTypes
                .Where(m => m.CompanyId == companyId && m.CompanyClientId == companyClientId)
                .ToListAsync(ct);

            var currentByTypeId = current.ToDictionary(x => x.InventoryTypeId, x => x);

            // 1) Ensure all provided IDs exist and are active
            foreach (var typeId in inventoryTypeIds)
            {
                if (currentByTypeId.TryGetValue(typeId, out var existing))
                {
                    if (!existing.IsActive)
                        existing.IsActive = true;
                }
                else
                {
                    _db.CompanyClientInventoryTypes.Add(new ModelsWMS.Masters.CompanyClientInventoryType
                    {
                        CompanyId = companyId,
                        CompanyClientId = companyClientId,
                        InventoryTypeId = typeId,
                        IsActive = true
                    });
                }
            }

            // 2) Deactivate anything not in final set
            var finalSet = inventoryTypeIds.ToHashSet();
            foreach (var m in current)
            {
                if (!finalSet.Contains(m.InventoryTypeId) && m.IsActive)
                    m.IsActive = false;
            }

            await _db.SaveChangesAsync(ct);
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(
            int companyId,
            int companyClientId,
            int inventoryTypeId,
            int? excludeId,
            CancellationToken ct)
        {
            var query = _db.CompanyClientInventoryTypes
                .AsNoTracking()
                .Where(m =>
                    m.CompanyId == companyId &&
                    m.CompanyClientId == companyClientId &&
                    m.InventoryTypeId == inventoryTypeId);

            if (excludeId.HasValue)
                query = query.Where(m => m.Id != excludeId.Value);

            return await query.AnyAsync(ct);
        }

        /// <inheritdoc/>
        public async Task<bool> IsMasterActiveAsync(
            int companyId,
            int inventoryTypeId,
            CancellationToken ct)
        {
            // If InventoryTypes is tenant-scoped, add: && t.CompanyId == companyId
            return await _db.InventoryTypes
                .AsNoTracking()
                .AnyAsync(t => t.Id == inventoryTypeId && t.Active, ct);
        }
    }
}
