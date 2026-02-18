using DUNES.API.Data;
using DUNES.Shared.DTOs.WMS;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.CompanyClientItemStatus
{
    /// <summary>
    /// Repository implementation for managing item status enablement per client.
    /// Strictly scoped by CompanyId + CompanyClientId (from token).
    /// </summary>
    public class CompanyClientItemStatusWMSAPIRepository : ICompanyClientItemStatusWMSAPIRepository
    {
        private readonly appWmsDbContext _db;

        /// <summary>Constructor (DI).</summary>
        public CompanyClientItemStatusWMSAPIRepository(appWmsDbContext db)
        {
            _db = db;
        }

        /// <inheritdoc/>
        public async Task<List<WMSCompanyClientItemStatusReadDTO>> GetEnabledAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            // Enabled = mapping active AND master active
            return await _db.CompanyClientItemStatuses
                .AsNoTracking()
                .Where(m => m.CompanyId == companyId
                         && m.CompanyClientId == companyClientId
                         && m.IsActive)
                .Join(
                    _db.Itemstatus.AsNoTracking().Where(s => s.Active),
                    m => m.ItemStatusId,
                    s => s.Id,
                    (m, s) => new WMSCompanyClientItemStatusReadDTO
                    {
                        Id = m.Id,
                        ItemStatusId = s.Id,
                        ItemStatusName = s.Name,
                        IsActive = m.IsActive
                    })
                .OrderBy(x => x.ItemStatusName)
                .ToListAsync(ct);
        }

        /// <inheritdoc/>
        public async Task<WMSCompanyClientItemStatusReadDTO?> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct)
        {
            // Consistent with enabled logic: require master active.
            return await _db.CompanyClientItemStatuses
                .AsNoTracking()
                .Where(m => m.Id == id
                         && m.CompanyId == companyId
                         && m.CompanyClientId == companyClientId)
                .Join(
                    _db.Itemstatus.AsNoTracking().Where(s => s.Active),
                    m => m.ItemStatusId,
                    s => s.Id,
                    (m, s) => new WMSCompanyClientItemStatusReadDTO
                    {
                        Id = m.Id,
                        ItemStatusId = s.Id,
                        ItemStatusName = s.Name,
                        IsActive = m.IsActive
                    })
                .FirstOrDefaultAsync(ct);
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(
            int companyId,
            int companyClientId,
            int itemStatusId,
            int? excludeId,
            CancellationToken ct)
        {
            var query = _db.CompanyClientItemStatuses
                .AsNoTracking()
                .Where(m =>
                    m.CompanyId == companyId &&
                    m.CompanyClientId == companyClientId &&
                    m.ItemStatusId == itemStatusId);

            if (excludeId.HasValue)
                query = query.Where(m => m.Id != excludeId.Value);

            return await query.AnyAsync(ct);
        }

        /// <inheritdoc/>
        public async Task<WMSCompanyClientItemStatusReadDTO> CreateAsync(
            WMSCompanyClientItemStatusCreateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            // Anti-error: master must be active
            var master = await _db.Itemstatus
                .AsNoTracking()
                .Where(s => s.Id == dto.ItemStatusId && s.Active)
                .Select(s => new { s.Id, s.Name })
                .FirstOrDefaultAsync(ct);

            if (master is null)
                throw new InvalidOperationException("Cannot create mapping: master item status is invalid or inactive.");

            // Anti-error: avoid duplicates (unique index should also exist)
            var exists = await _db.CompanyClientItemStatuses
                .AsNoTracking()
                .AnyAsync(m =>
                    m.CompanyId == companyId &&
                    m.CompanyClientId == companyClientId &&
                    m.ItemStatusId == dto.ItemStatusId,
                    ct);

            if (exists)
                throw new InvalidOperationException("Mapping already exists for this client and item status.");

            var entity = new ModelsWMS.Masters.CompanyClientItemStatus
            {
                CompanyId = companyId,
                CompanyClientId = companyClientId,
                ItemStatusId = dto.ItemStatusId,
                IsActive = dto.IsActive
            };

            _db.CompanyClientItemStatuses.Add(entity);
            await _db.SaveChangesAsync(ct);

            return new WMSCompanyClientItemStatusReadDTO
            {
                Id = entity.Id,
                ItemStatusId = master.Id,
                ItemStatusName = master.Name,
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
            var entity = await _db.CompanyClientItemStatuses
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
                var masterActive = await IsMasterActiveAsync(companyId, entity.ItemStatusId, ct);
                if (!masterActive)
                    throw new InvalidOperationException("Cannot activate mapping: master item status is inactive.");
            }

            entity.IsActive = isActive;
            await _db.SaveChangesAsync(ct);
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> SetEnabledSetAsync(
            int companyId,
            int companyClientId,
            List<int> itemStatusIds,
            CancellationToken ct)
        {
            itemStatusIds ??= new List<int>();
            itemStatusIds = itemStatusIds.Where(x => x > 0).Distinct().ToList();

            // Validate all provided IDs are master-active
            if (itemStatusIds.Count > 0)
            {
                var activeIds = await _db.Itemstatus
                    .AsNoTracking()
                    .Where(s => s.Active && itemStatusIds.Contains(s.Id))
                    .Select(s => s.Id)
                    .ToListAsync(ct);

                var missingOrInactive = itemStatusIds.Except(activeIds).ToList();
                if (missingOrInactive.Count > 0)
                    throw new InvalidOperationException($"Invalid/inactive master item statuses: {string.Join(",", missingOrInactive)}");
            }

            // Load current mappings
            var current = await _db.CompanyClientItemStatuses
                .Where(m => m.CompanyId == companyId && m.CompanyClientId == companyClientId)
                .ToListAsync(ct);

            var currentByStatusId = current.ToDictionary(x => x.ItemStatusId, x => x);

            // 1) Ensure all provided IDs exist and are active
            foreach (var statusId in itemStatusIds)
            {
                if (currentByStatusId.TryGetValue(statusId, out var existing))
                {
                    if (!existing.IsActive)
                        existing.IsActive = true;
                }
                else
                {
                    _db.CompanyClientItemStatuses.Add(new ModelsWMS.Masters.CompanyClientItemStatus
                    {
                        CompanyId = companyId,
                        CompanyClientId = companyClientId,
                        ItemStatusId = statusId,
                        IsActive = true
                    });
                }
            }

            // 2) Deactivate anything not included
            var finalSet = itemStatusIds.ToHashSet();
            foreach (var m in current)
            {
                if (!finalSet.Contains(m.ItemStatusId) && m.IsActive)
                    m.IsActive = false;
            }

            await _db.SaveChangesAsync(ct);
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> IsMasterActiveAsync(
            int companyId,
            int itemStatusId,
            CancellationToken ct)
        {
            // If Itemstatus is tenant-scoped, add: && s.CompanyId == companyId
            return await _db.Itemstatus
                .AsNoTracking()
                .AnyAsync(s => s.Id == itemStatusId && s.Active, ct);
        }
    }
}
